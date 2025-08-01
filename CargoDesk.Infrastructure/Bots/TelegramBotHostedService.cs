using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using IUpdateHandler = CargoDesk.Application.Services.Telegram.IUpdateHandler;

namespace CargoDesk.Infrastructure.Bots;

public class TelegramBotHostedService : BackgroundService
{
    private readonly ITelegramBotClient _bot;
    private readonly ILogger<TelegramBotHostedService> _log;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly MenuCommandInitializer _menuInitializer;

    public TelegramBotHostedService(ITelegramBotClient bot, ILogger<TelegramBotHostedService> log, IServiceScopeFactory scopeFactory, MenuCommandInitializer menuInitializer)
    {
        _bot = bot;
        _log = log;
        _scopeFactory = scopeFactory;
        _menuInitializer = menuInitializer;
    }

    protected override async  Task ExecuteAsync(CancellationToken ct)
    {
        _log.LogInformation("Starting Telegram bot...");

        await _menuInitializer.InitializeMenuAsync(ct);

        _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions: new ReceiverOptions { AllowedUpdates = { } },
                cancellationToken: ct
            );

        var me =  await _bot.GetMe(ct);
        _log.LogInformation("Bot @{Username} is ready", me.Username);
        await Task.Delay(Timeout.Infinite, ct);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IUpdateHandler>();
        foreach (var h in handlers)
        {
            if (await h.CanHandleAsync(update, ct))
            {
                await h.HandleAsync(bot, update, ct);
                break;
            }
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        _log.LogError(ex, "Error in Telegram Bot");
        return Task.CompletedTask;
    }
}