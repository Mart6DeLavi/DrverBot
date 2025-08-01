using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class DelayRequestHandler : IUpdateHandler
{
    private readonly IDriverChatMappingService _chatMap;
    private readonly IDelayRequestRepository _delayRepo;

    private static readonly HashSet<long> _waitingForDelayTime = new();

    public DelayRequestHandler(IDriverChatMappingService chatMap, IDelayRequestRepository delayRepo)
    {
        _chatMap = chatMap;
        _delayRepo = delayRepo;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        if (update.Type == UpdateType.Message)
        {
            var txt = update.Message?.Text;
            var chatId = update.Message.Chat.Id;
            return Task.FromResult(
                txt == "⏳ Delay" || txt == "/delay" || txt == "Delay" || _waitingForDelayTime.Contains(chatId)
            );
        }
        if (update.Type == UpdateType.CallbackQuery)
        {
            return Task.FromResult(update.CallbackQuery?.Data?.StartsWith("delay:") == true);
        }
        return Task.FromResult(false);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var msg = update.Message!;
            var chatId = msg.Chat.Id;

            if ((msg.Text == "⏳ Delay" || msg.Text == "/delay" || msg.Text == "Delay") && !_waitingForDelayTime.Contains(chatId))
            {
                _waitingForDelayTime.Add(chatId);

                string delayRequestText =
                    "Please specify your expected delay time or select one of the options below.\n\n" +
                    "*Examples:* `20 min`, `until 13:00`, `45 minutes`, `1 hour`\n\n" +
                    "You can enter a custom value, or choose one of the quick options:";

                var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🕐 1 hour", "delay:1h"),
                        InlineKeyboardButton.WithCallbackData("🕑 2 hours", "delay:2h"),
                        InlineKeyboardButton.WithCallbackData("🕒 3 hours", "delay:3h"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🕔 5 hours", "delay:5h"),
                        InlineKeyboardButton.WithCallbackData("🕛 12 hours", "delay:12h"),
                        InlineKeyboardButton.WithCallbackData("📅 1 Day", "delay:24h"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("✏️ Custom", "delay:custom"),
                    }
                });

                await bot.SendMessage(
                    chatId: chatId,
                    text: delayRequestText,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: keyboard,
                    cancellationToken: ct
                );
                return;
            }

            if (_waitingForDelayTime.Contains(chatId))
            {
                _waitingForDelayTime.Remove(chatId);

                var delayText = msg.Text?.Trim() ?? "(not specified)";

                var driverId = await _chatMap.GetDriverIdByChatIdAsync(chatId, ct);
                if (driverId is null)
                {
                    await bot.SendMessage(
                        chatId: chatId,
                        text: "User not found.",
                        cancellationToken: ct
                    );
                    return;
                }

                var delayRequest = new DelayRequestEntity
                {
                    Id = Guid.NewGuid(),
                    DriverId = driverId.Value,
                    ChatId = chatId,
                    DispatcherId = Guid.Empty,
                    DelayTime = delayText,
                    CreatedAt = DateTime.UtcNow
                };
                await _delayRepo.CreateAsync(delayRequest, ct);

                await bot.SendMessage(
                    chatId: chatId,
                    text: $"Your delay notification has been sent to the dispatcher.\n" +
                          $"Reported delay: *{delayText}*",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: ct
                );
            }
        }

        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery?.Data?.StartsWith("delay:") == true)
        {
            var cq = update.CallbackQuery!;
            var chatId = cq.Message!.Chat.Id;
            var data = cq.Data!;

            if (data == "delay:custom")
            {
                _waitingForDelayTime.Add(chatId);
                await bot.SendMessage(
                    chatId: chatId,
                    text: "Please enter your custom delay time (e.g. 20 min, until 13:00):",
                    cancellationToken: ct
                );
                return;
            }

            string delayText = data switch
            {
                "delay:1h" => "1 hour",
                "delay:2h" => "2 hours",
                "delay:3h" => "3 hours",
                "delay:5h" => "5 hours",
                "delay:12h" => "12 hours",
                "delay:24h" => "1 Day",
                _ => "(not specified)"
            };

            var driverId = await _chatMap.GetDriverIdByChatIdAsync(chatId, ct);
            if (driverId is null)
            {
                await bot.SendMessage(
                    chatId: chatId,
                    text: "User not found.",
                    cancellationToken: ct
                );
                return;
            }

            var delayRequest = new DelayRequestEntity
            {
                Id = Guid.NewGuid(),
                DriverId = driverId.Value,
                ChatId = chatId,
                DispatcherId = Guid.Empty,
                DelayTime = delayText,
                CreatedAt = DateTime.UtcNow
            };

            await _delayRepo.CreateAsync(delayRequest, ct);

            await bot.SendMessage(
                chatId: chatId,
                text: $"Your delay notification has been sent to the dispatcher.\n" +
                      $"Reported delay: *{delayText}*",
                parseMode: ParseMode.Markdown,
                cancellationToken: ct
            );
        }
    }
}