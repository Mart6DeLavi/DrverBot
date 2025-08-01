using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Bots.Helpers;
using CargoDesk.Infrastructure.Persistence.Enums;
using CargoDesk.Infrastructure.Persistence.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class BackToAllCargosHandler : IUpdateHandler
{
    private readonly IDriverChatMappingService _chatMapping;
    private readonly IRouteRepository _routeRepository;
    private readonly ICargoRepository _cargoRepository;
    private readonly IRouteCargoStatusRepository _statusRepository;

    public BackToAllCargosHandler(IDriverChatMappingService chatMapping, IRouteRepository routeRepository,
        ICargoRepository cargoRepository, IRouteCargoStatusRepository statusRepository)
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
            update.Message?.Text == "⬅️ Back to all cargos"
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;

        SelectedCargoMemory.SelectedCargoByChatId.TryRemove(chatId, out _);
        StatusUpdateHandler.ClearActiveCargo(chatId);

        var driverId = await _chatMapping.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId is null)
        {
            await bot.SendMessage(chatId, "Driver not found.", cancellationToken: ct);
            return;
        }
        var routes = await _routeRepository.GetAllRoutesByDriver(driverId.Value, ct);
        var routeIds = routes.Select(r => r.Id).ToList();
        var cargoIds = routes.SelectMany(r => r.CargoIds).Distinct().ToList();

        var cargos = await _cargoRepository.GetCargosByIds(cargoIds, ct);
        var statuses = await _statusRepository.GetByRoutesAsync(routeIds, ct);

        var cargoAndStatuses = cargos.Select(cargo =>
        {
            var routeStatus = statuses
                .FirstOrDefault(s => s.CargoId == cargo.Id)?.Status
                ?? RouteStatus.Assigned;
            return (Cargo: cargo, Status: routeStatus);
        }).ToList();

        var keyboard = KeyboardHelpers.BuildMainCargoKeyboard(cargoAndStatuses);

        await bot.SendMessage(
            chatId: chatId,
            text: "Select a cargo for detailed information or further actions:",
            replyMarkup: keyboard,
            cancellationToken: ct
        );
    }
}