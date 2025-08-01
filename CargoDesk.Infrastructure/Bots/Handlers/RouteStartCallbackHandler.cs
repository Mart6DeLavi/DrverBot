using CargoDesk.Application.Services.Telegram;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Bots.Helpers;
using CargoDesk.Infrastructure.Persistence.Enums;
using CargoDesk.Infrastructure.Persistence.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class RouteStartCallbackHandler : IUpdateHandler
{
    private readonly IRouteService _routeService;
    private readonly IDriverChatMappingService _chatMap;
    private readonly ICargoRepository _cargoRepository;
    private readonly IDriverWorkSessionRepository _workSessionRepository;
    private readonly IRouteCargoStatusRepository _routeCargoStatusRepository;

    public RouteStartCallbackHandler(IRouteService routeService, IDriverChatMappingService chatMap, ICargoRepository cargoRepository, IDriverWorkSessionRepository workSessionRepository, IRouteCargoStatusRepository routeCargoStatusRepository)
    {
        _routeService = routeService;
        _chatMap = chatMap;
        _cargoRepository = cargoRepository;
        _workSessionRepository = workSessionRepository;
        _routeCargoStatusRepository = routeCargoStatusRepository;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
            && update.CallbackQuery?.Data?.StartsWith("start:") == true
        );
    }

    public async Task HandleAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var cq = update.CallbackQuery!;
        var chatId = cq.Message!.Chat.Id;
        var driverId = await _chatMap.GetDriverIdByChatIdAsync(chatId, ct);
        var parts = cq.Data!.Split(':');
        if (parts.Length < 2)
        {
            await botClient.SendMessage(
                chatId: chatId,
                text: "Error: failed to determine the route.",
                cancellationToken: ct
            );
            return;
        }

        var routeId = Guid.Parse(parts[1]);

        if (driverId != null)
        {
            await _workSessionRepository.StartWorkAsync(driverId.Value, routeId, ct);
        }

        var route = await _routeService.GetRouteById(routeId, ct);
        var cargoIds = route.CargoIds;

        if (cargoIds != null && cargoIds.Count > 0)
        {
            foreach (var cargoId in cargoIds)
            {
                var cargo = await _cargoRepository.GetCargoById(cargoId, ct);
                var refNum = cargo.ReferenceNumber;

                var textHtml = $@"🚚 <b>Cargo</b> <i>{refNum}</i>
⌚ <b>Planned pickup time:</b> {cargo.PlannedPickUpDateTime:yyyy-MM-dd HH:mm}
⌚ <b>Planned delivery time:</b> {cargo.PlannedDeliveryDateTime:yyyy-MM-dd HH:mm}
📦 <b>Number of pallets:</b> {cargo.NumberOfPallets}
⚖️ <b>Weight:</b> {cargo.Weight} kg
<i>{refNum}</i>";

                await botClient.SendMessage(
                    chatId: chatId,
                    text: textHtml,
                    parseMode: ParseMode.Html,
                    cancellationToken: ct
                );
            }

            var cargos = await _cargoRepository.GetCargosByIds(cargoIds, ct);
            var statuses = await _routeCargoStatusRepository.GetByRouteAsync(routeId, ct);

            var cargoAndStatuses = cargos.Select(cargo =>
            {
                var cargoStatus = statuses.FirstOrDefault(s => s.CargoId == cargo.Id)?.Status ?? RouteStatus.Assigned;
                return (Cargo: cargo, route: cargoStatus);
            }).ToList();

            var mainKeyboard = KeyboardHelpers.BuildMainCargoKeyboard(cargoAndStatuses);

            await botClient.SendMessage(
                chatId: chatId,
                text: "Please select a cargo for more details or further actions:",
                replyMarkup: mainKeyboard,
                cancellationToken: ct
            );
        }
    }
}
