using CargoDesk.Infrastructure.Persistence.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Application.Services.Telegram;

public class RouteNotificationService : IRouteNotificationService
{
    private readonly IDriverChatMappingService _driverChat;
    private readonly ITelegramBotClient _bot;
    private readonly IRouteRepository _routeRepository;
    private readonly ICargoRepository _cargoRepository;

    public RouteNotificationService(
        IDriverChatMappingService driverChat,
        ITelegramBotClient bot,
        IRouteRepository routeRepository,
        ICargoRepository cargoRepository)
    {
        _driverChat = driverChat;
        _bot = bot;
        _routeRepository = routeRepository;
        _cargoRepository = cargoRepository;
    }

    public async Task NotifyDriverRouteCreatedAsync(Guid driverId, Guid routeId, CancellationToken ct)
    {
        var maybeChatId = await _driverChat.GetChatIdByDriverIdAsync(driverId, ct);
        if (!maybeChatId.HasValue) return;
        var chatId = maybeChatId.Value;

        var text = "🚚 <b>You have been assigned a new route!</b>\n" +
                   "Press the button below to start work.";

        var inlineKeyboard = new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("✅ Start route", $"start:{routeId}")
        );

        await _bot.SendMessage(
            chatId: chatId,
            text: text,
            parseMode: ParseMode.Html,
            replyMarkup: inlineKeyboard,
            cancellationToken: ct
        );
    }
}
