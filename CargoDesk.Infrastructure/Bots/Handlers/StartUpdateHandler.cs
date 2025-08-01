using CargoDesk.Application.Services.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class StartUpdateHandler : IUpdateHandler
{
    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.Message
            && update.Message?.Type == MessageType.Text
            && update.Message.Text.Trim() == "/start"
        );
    }

    public Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            KeyboardButton.WithRequestContact("Give me your number"),
        })
        {
            OneTimeKeyboard = true,
            ResizeKeyboard = true
        };

        return bot.SendMessage(
            chatId: chatId,
            text: "Welcome. Please give us your number that we can recognize you",
            replyMarkup: keyboard,
            cancellationToken: ct
        );
    }
}