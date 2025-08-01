using System.Collections.Concurrent;
using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Bots.Helpers;
using CargoDesk.Infrastructure.Persistence.Enums;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class StatusUpdateHandler : IUpdateHandler
{
    private static readonly RouteStatus[] _allStatuses =
        (RouteStatus[])Enum.GetValues(typeof(RouteStatus));

    private static readonly ConcurrentDictionary<(long chatId, Guid cargoId), RouteStatus> _currentStatus
        = new();

    private static readonly IReadOnlyDictionary<RouteStatus, string> _emoji =
        new Dictionary<RouteStatus, string>
    {
        [RouteStatus.Assigned]            = "🟢",
        [RouteStatus.InTransitToPickup]   = "🟡",
        [RouteStatus.AtPickupPoint]       = "🔵",
        [RouteStatus.Loading]             = "⚙️",
        [RouteStatus.LoadingCompleted]    = "✅",
        [RouteStatus.AtDeliveryPoint]     = "🔷",
        [RouteStatus.Unloading]           = "📦",
        [RouteStatus.UnloadingCompleted]  = "🏁"
    };

    private static readonly ConcurrentDictionary<long, (Guid routeId, Guid cargoId)> _activeCargoForChat
        = new();

    private static readonly ConcurrentDictionary<(long chatId, Guid routeId, Guid cargoId), PendingStatusChange> _pendingStatus
        = new();

    public static TimeSpan StatusChangeDelay = TimeSpan.FromSeconds(60);

    public static string GetEmoji(RouteStatus status) =>
        _emoji.TryGetValue(status, out var emoji) ? emoji : "";

    public static void SetActiveCargo(long chatId, Guid routeId, Guid cargoId)
    {
        _activeCargoForChat[chatId] = (routeId, cargoId);
    }

    public static void ClearActiveCargo(long chatId)
    {
        _activeCargoForChat.TryRemove(chatId, out _);
    }

    private readonly IDriverChatMappingService _chatMap;
    private readonly IServiceScopeFactory _scopeFactory;

    public StatusUpdateHandler(IDriverChatMappingService chatMap, IServiceScopeFactory scopeFactory)
    {
        _chatMap = chatMap;
        _scopeFactory = scopeFactory;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message)
            return Task.FromResult(false);

        var txt = update.Message?.Text?.Trim();
        if (string.IsNullOrEmpty(txt))
            return Task.FromResult(false);

        return Task.FromResult(StatusDisplay.Lookup.ContainsKey(txt));
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        var text = update.Message.Text!.Trim();

        if (!StatusDisplay.Lookup.TryGetValue(text, out var current))
            return;

        var driverId = await _chatMap.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId is null)
        {
            await botClient.SendMessage(
                chatId: chatId,
                text: "User not found.",
                cancellationToken: ct
            );
            return;
        }

        if (!_activeCargoForChat.TryGetValue(chatId, out var pair))
        {
            await botClient.SendMessage(
                chatId: chatId,
                text: "No selected cargo. Please choose cargo first.",
                cancellationToken: ct
            );
            return;
        }

        var (routeId, cargoId) = pair;
        var idx = Array.IndexOf(_allStatuses, current);
        var next = idx < _allStatuses.Length - 1
            ? _allStatuses[idx + 1]
            : current;

        _currentStatus[(chatId, cargoId)] = current;
        _pendingStatus.TryRemove((chatId, routeId, cargoId), out _);

        var ctsLocal = new CancellationTokenSource();
        _pendingStatus[(chatId, routeId, cargoId)] = new PendingStatusChange
        {
            RouteId = routeId,
            CargoId = cargoId,
            NewStatus = next,
            CancellationTokenSource = ctsLocal
        };

        var inlineKb = new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                text: "❌ Cancel",
                callbackData: $"cancel_status:{cargoId}"
            )
        );

        await botClient.SendMessage(
            chatId: chatId,
            text: $"The cargo status will be changed to *{StatusDisplay.Display[current]}* in 1 minute.\n" +
                  "If you want to cancel this change, press ❌ below.",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            replyMarkup: inlineKb,
            cancellationToken: ct
        );

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(StatusChangeDelay, ctsLocal.Token);
                if (ctsLocal.IsCancellationRequested) return;

                using var scope = _scopeFactory.CreateScope();
                var statusRepo = scope.ServiceProvider
                    .GetRequiredService<IRouteCargoStatusRepository>();

                await statusRepo.UpdateStatusAsync(
                    routeId,
                    cargoId,
                    next,
                    CancellationToken.None
                );

                if (next == RouteStatus.UnloadingCompleted)
                {
                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"🚚 Status is now *Unloading Completed*. The work is finished. Thank you!",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyMarkup: GetStatusKeyboard(RouteStatus.UnloadingCompleted),
                        cancellationToken: ct
                    );
                    ClearActiveCargo(chatId);
                }
                else
                {
                    _currentStatus[(chatId, cargoId)] = next;

                    await botClient.SendMessage(
                        chatId: chatId,
                        text: $"Status updated to *{StatusDisplay.Display[current]}*.",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyMarkup: GetStatusKeyboard(next),
                        cancellationToken: ct
                    );
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StatusUpdateHandler] ERROR in background task: {ex}");
            }
            finally
            {
                _pendingStatus.TryRemove((chatId, routeId, cargoId), out _);
            }
        });
    }


    public static ReplyKeyboardMarkup GetStatusKeyboard(RouteStatus status)
    {
        var emoji = GetEmoji(status);

        if (status == RouteStatus.UnloadingCompleted)
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("⬅️ Back to all cargos") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };
        }

        var label = StatusDisplay.Display[status];
        return new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton(label) },
            new[]
            {
                new KeyboardButton("⬅️ Back to all cargos"),
                new KeyboardButton("🔍 Cargo info")
            },
            new[]
            {
                new KeyboardButton("⚠️ Report issue"),
                new KeyboardButton("⏸️ Finish work")
            }
        })
        {
            ResizeKeyboard  = true,
            OneTimeKeyboard = false
        };
    }

    public static bool TryCancelPendingByChatAndCargo(long chatId, Guid cargoId)
    {
        var key = _pendingStatus.Keys
            .FirstOrDefault(k => k.chatId == chatId && k.cargoId == cargoId);

        if (key != default && _pendingStatus.TryRemove(key, out var pend))
        {
            pend.CancellationTokenSource.Cancel();
            return true;
        }
        return false;
    }

    public static RouteStatus? ActiveStatusFor(long chatId, Guid cargoId)
    {
        if (_currentStatus.TryGetValue((chatId, cargoId), out var st))
            return st;
        return null;
    }
}