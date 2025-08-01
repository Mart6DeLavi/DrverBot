using Telegram.Bot;
using Telegram.Bot.Types;

namespace CargoDesk.Infrastructure.Bots;

public class MenuCommandInitializer
{
    private readonly ITelegramBotClient _bot;

    public MenuCommandInitializer(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task InitializeMenuAsync(CancellationToken ct)
    {
        await _bot.SetMyCommands(new[]
        {
            new Telegram.Bot.Types.BotCommand
            { Command = "report_issue", Description = "⚠️ Report an issue" },
            new BotCommand { Command = "delay", Description = "⏳ Report a delay"},
            new BotCommand { Command = "finish_work", Description = "⏸️ Finish work"},
            new BotCommand { Command = "instruction", Description = "📕 Instructions"}
        }, cancellationToken: ct);
    }
}