using CargoDesk.Application.Services.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class FallbackHandler : IUpdateHandler
{
    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(update.Type == UpdateType.Message
            && update.Message?.Text != null);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        await bot.SendMessage(
            chatId: chatId,
            text: "🚫 Unknown command. Please use the buttons to navigate.",
            cancellationToken: ct
        );
    }
}