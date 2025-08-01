using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Bots.Helpers;
using CargoDesk.Infrastructure.Persistence.Enums;
using CargoDesk.Infrastructure.Persistence.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class ShowCargoInfoHandler : IUpdateHandler
{
    private readonly IDriverChatMappingService _chatMapping;
    private readonly IRouteRepository _routeRepository;
    private readonly ICargoRepository _cargoRepository;
    private readonly IRouteCargoStatusRepository _statusRepository;

    public ShowCargoInfoHandler(IDriverChatMappingService chatMapping, IRouteRepository routeRepository, ICargoRepository cargoRepository, IRouteCargoStatusRepository statusRepository)
    {
        _chatMapping = chatMapping;
        _routeRepository = routeRepository;
        _cargoRepository = cargoRepository;
        _statusRepository = statusRepository;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.Message &&
            update.Message?.Text == "🔍 Cargo info"
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;

        if (!SelectedCargoMemory.SelectedCargoByChatId.TryGetValue(chatId, out var cargoId))
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "No selected cargo. Please choose one first.",
                cancellationToken: ct
            );
            return;
        }

        var driverId = await _chatMapping.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId is null)
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "Driver not found",
                cancellationToken: ct
            );
            return;
        }

        var routes = await _routeRepository.GetAllRoutesByDriver(driverId.Value, ct);
        var routeIds = routes.Select(r => r.Id).ToList();
        var cargoIds = routes.SelectMany(r => r.CargoIds).Distinct().ToList();
        var cargos = await _cargoRepository.GetCargosByIds(cargoIds, ct);
        var cargo = cargos.FirstOrDefault(c => c.Id == cargoId);

        if (cargo is null)
        {
            await bot.SendMessage(
                chatId: chatId,
                "Cargo not found in your routes",
                cancellationToken: ct
            );
            return;
        }

        var statuses = await _statusRepository.GetByRoutesAsync(routeIds, ct);
        var status = statuses.FirstOrDefault(s => s.CargoId == cargoId)?.Status
                     ?? RouteStatus.Assigned;

        var textHtml = $@"🚚 <b>Cargo</b> <i>{cargo.ReferenceNumber}</i>
⌚ <b>Planned pickup:</b>   {cargo.PlannedPickUpDateTime:yyyy-MM-dd HH:mm}
⌚ <b>Planned delivery:</b> {cargo.PlannedDeliveryDateTime:yyyy-MM-dd HH:mm}
📦 <b>Pallets:</b>          {cargo.NumberOfPallets}
⚖️ <b>Weight:</b>           {cargo.Weight} kg
<i>{cargo.ReferenceNumber}</i>";

        var keyboard = StatusUpdateHandler.GetStatusKeyboard(status);

        await bot.SendMessage(
            chatId: chatId,
            text: textHtml,
            parseMode: ParseMode.Html,
            replyMarkup: keyboard,
            cancellationToken: ct
        );
    }
}