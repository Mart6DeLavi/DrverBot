using System.Text;
using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Bots.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class InstructionCommandHandler : IUpdateHandler
{
    private const string InstructionCommand = "/instruction";

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.Message &&
            update.Message!.Text == InstructionCommand
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;

        var sb = new StringBuilder();
        sb.AppendLine("📕 *Cargo Status Emoji Instructions*");
        sb.AppendLine();

        foreach (var kv in StatusDisplay.Display)
        {
            sb.AppendLine(kv.Value);
        }

        await bot.SendMessage(
            chatId: chatId,
            text: sb.ToString(),
            parseMode: ParseMode.Markdown,
            cancellationToken: ct
        );
    }
}