using CargoDesk.Application.Services.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class FinishWorkHandler : IUpdateHandler
{
    private readonly IDriverChatMappingService _chatMapping;
    private readonly IDriverWorkSessionRepository _workSessionRepository;

    public FinishWorkHandler(IDriverChatMappingService chatMapping, IDriverWorkSessionRepository workSessionRepository)
    {
        _chatMapping = chatMapping;
        _workSessionRepository = workSessionRepository;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.Message &&
            (update.Message?.Text == "⏸️ Finish work" || update.Message?.Text == "/finish_work")
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        var driverId = await _chatMapping.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId == null)
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "User not found.",
                cancellationToken: ct
            );
            return;
        }

        await _workSessionRepository.FinishWorkAsync(driverId.Value, ct);

        await bot.SendMessage(
            chatId: chatId,
            text: "Keyboard hidden.",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: ct
        );

        var inlineKeyboard = new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("▶️ Resume work", "resume_work")
        );

        await bot.SendMessage(
            chatId: chatId,
            text: "Your work session has been ended. \nWhen you want to resume work, press the button below.",
            replyMarkup: inlineKeyboard,
            cancellationToken: ct
        );
    }
}