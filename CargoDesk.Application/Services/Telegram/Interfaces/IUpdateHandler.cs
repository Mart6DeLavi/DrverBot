using Telegram.Bot;
using Telegram.Bot.Types;

namespace CargoDesk.Application.Services.Telegram;

public interface IUpdateHandler
{
    Task<bool> CanHandleAsync(Update update, CancellationToken ct);
    Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct);
}