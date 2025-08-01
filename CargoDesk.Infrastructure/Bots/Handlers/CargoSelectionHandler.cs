using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Bots.Helpers;
using CargoDesk.Infrastructure.Persistence.Enums;
using CargoDesk.Infrastructure.Persistence.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class CargoSelectionHandler : IUpdateHandler
{
    private readonly IDriverChatMappingService _chatMapping;
    private readonly IRouteRepository _routeRepository;
    private readonly ICargoRepository _cargoRepository;

    public CargoSelectionHandler(IDriverChatMappingService chatMapping, IRouteRepository routeRepository, ICargoRepository cargoRepository)
    {
        _chatMapping = chatMapping;
        _routeRepository = routeRepository;
        _cargoRepository = cargoRepository;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message) return Task.FromResult(false);
        var text = update.Message?.Text?.Trim();
        if (string.IsNullOrEmpty(text)) return Task.FromResult(false);

        var parts = text.Split(' ', 2);
        if (parts.Length != 2) return Task.FromResult(false);

        var isEmoji = StatusEmoji.AllEmojis.Contains(parts[0]);
        var isRef   = parts[1].StartsWith("REF-", StringComparison.OrdinalIgnoreCase);

        return Task.FromResult(isEmoji && isRef);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.Message!.Chat.Id;
        var text   = update.Message.Text!.Trim();
        var parts  = text.Split(' ', 2);

        if (!StatusEmoji.TryGetStatus(parts[0], out var currentStatus))
            return;

        var refNum = parts[1];

        var driverId = await _chatMapping.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId is null)
        {
            await bot.SendMessage(chatId, "Driver not found.", cancellationToken: ct);
            return;
        }

        var routes   = await _routeRepository.GetAllRoutesByDriver(driverId.Value, ct);
        var routeIds = routes.Select(r => r.Id).ToList();
        var cargoIds = routes.SelectMany(r => r.CargoIds).Distinct().ToList();

        var cargos = await _cargoRepository.GetCargosByIds(cargoIds, ct);
        var cargo  = cargos.FirstOrDefault(c =>
            c.ReferenceNumber.Equals(refNum, StringComparison.OrdinalIgnoreCase));
        if (cargo is null)
        {
            await bot.SendMessage(chatId, "Cargo not found.", cancellationToken: ct);
            return;
        }

        if (currentStatus == RouteStatus.UnloadingCompleted)
        {
            await bot.SendMessage(chatId,
                "Work on this cargo is already finished.",
                cancellationToken: ct);
            return;
        }

        var route = routes.First(r => r.CargoIds.Contains(cargo.Id));
        SelectedCargoMemory.SelectedCargoByChatId[chatId] = cargo.Id;
        StatusUpdateHandler.SetActiveCargo(chatId, route.Id, cargo.Id);

        var keyboard = KeyboardHelpers.BuildCargoStatusKeyboard(currentStatus);

        await bot.SendMessage(
            chatId:      chatId,
            text:        $"Cargo <b>{refNum}</b> selected.",
            parseMode:   ParseMode.Html,
            replyMarkup: keyboard,
            cancellationToken: ct
        );
    }
}
