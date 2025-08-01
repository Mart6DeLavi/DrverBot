using CargoDesk.Application.Services.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class ReportIssueRequestHandler : IUpdateHandler
{
    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == Telegram.Bot.Types.Enums.UpdateType.Message &&
            (
                update.Message?.Text == "⚠️ Report issue" ||
                update.Message?.Text == "/report_issue" ||
                update.Message?.Text == "Report Issue"
            )
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;

        var inlineKb = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("🚗 Traffic jam",   "issue:TrafficJam"),
                InlineKeyboardButton.WithCallbackData("🔧 Breakdown on road", "issue:Breakdown")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("📝 Other",          "issue:Other")
            }
        });

        await bot.SendMessage(
            chatId:      chatId,
            text:        "Please choose the type of problem on route:",
            replyMarkup: inlineKb,
            cancellationToken: ct
        );
    }
}