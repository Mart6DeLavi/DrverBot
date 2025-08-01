using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class CancelStatusChangeHandler : IUpdateHandler
{
    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.CallbackQuery &&
            update.CallbackQuery?.Data?.StartsWith("cancel_status:") == true
        );
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var cq     = update.CallbackQuery!;
        var chatId = cq.Message!.Chat.Id;

        var parts = cq.Data!.Split(':', 2);
        if (parts.Length != 2 || !Guid.TryParse(parts[1], out var cargoId))
        {
            await botClient.SendMessage(
                chatId: chatId,
                text:   "Invalid data for cancel",
                cancellationToken: ct
            );
            return;
        }

        var canceled = StatusUpdateHandler.TryCancelPendingByChatAndCargo(chatId, cargoId);

        if (canceled)
        {
            await botClient.SendMessage(
                chatId: chatId,
                text:   "Status change has been cancelled.",
                cancellationToken: ct
            );

            if (StatusUpdateHandler.ActiveStatusFor(chatId, cargoId) is RouteStatus current)
            {
                await botClient.SendMessage(
                    chatId:    chatId,
                    text:      $"Current status: *{current}*",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: StatusUpdateHandler.GetStatusKeyboard(current),
                    cancellationToken: ct
                );
            }
        }
        else
        {
            await botClient.SendMessage(
                chatId: chatId,
                text:   "No pending status change found.",
                cancellationToken: ct
            );
        }
    }
}