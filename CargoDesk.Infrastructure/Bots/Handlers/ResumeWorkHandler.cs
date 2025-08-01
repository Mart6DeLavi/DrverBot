using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Bots.Helpers;
using CargoDesk.Infrastructure.Persistence.Enums;
using CargoDesk.Infrastructure.Persistence.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class ResumeWorkHandler : IUpdateHandler
{
    private readonly IRouteRepository _routeRepository;
    private readonly IDriverChatMappingService _chatMapping;
    private readonly IDriverWorkSessionRepository _workSessionRepository;
    private readonly IRouteCargoStatusRepository _statusRepo;
    private readonly ICargoRepository _cargoRepo;

    public ResumeWorkHandler(IRouteRepository routeRepository, IDriverChatMappingService chatMapping,
        IDriverWorkSessionRepository workSessionRepository, IRouteCargoStatusRepository statusRepo, ICargoRepository cargoRepo)
    {
        _routeRepository = routeRepository;
        _chatMapping = chatMapping;
        _workSessionRepository = workSessionRepository;
        _statusRepo = statusRepo;
        _cargoRepo = cargoRepo;
    }

    public Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return Task.FromResult(
            update.Type == UpdateType.CallbackQuery &&
            update.CallbackQuery?.Data == "resume_work"
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.CallbackQuery!.Message!.Chat.Id;
        var driverId = await _chatMapping.GetDriverIdByChatIdAsync(chatId, ct);
        if (driverId == null)
        {
            await bot.SendMessage(
                chatId: chatId,
                text:   "User not found",
                cancellationToken: ct
            );
            return;
        }

        var lastSession = await _workSessionRepository.GetLastSessionAsync(driverId.Value, ct);
        if (lastSession == null)
        {
            await bot.SendMessage(
                chatId: chatId,
                text:   "No previous work session found.",
                cancellationToken: ct
            );
            return;
        }

        var activeRoutes = await _routeRepository.GetAllRoutesByDriver(driverId.Value, ct);
        if (activeRoutes.Count == 0)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("⚠️ Report issue"),
                    new KeyboardButton("⏸️ Finish work")
                }
            })
            {
                ResizeKeyboard  = true,
                OneTimeKeyboard = true
            };

            await bot.SendMessage(
                chatId:      chatId,
                text:        "You have no active cargo. Please select an action:",
                replyMarkup: keyboard,
                cancellationToken: ct
            );
            return;
        }

        await _workSessionRepository.StartWorkAsync(driverId.Value, lastSession.RouteId, ct);

        SelectedCargoMemory.SelectedCargoByChatId.TryRemove(chatId, out _);
        StatusUpdateHandler.ClearActiveCargo(chatId);

        var routes   = activeRoutes;
        var routeIds = routes.Select(r => r.Id).ToList();
        var cargoIds = routes.SelectMany(r => r.CargoIds).Distinct().ToList();

        var cargos   = await _cargoRepo.GetCargosByIds(cargoIds, ct);
        var statuses = await _statusRepo.GetByRoutesAsync(routeIds, ct);

        var cargoAndStatuses = cargos.Select(c =>
        {
            var st = statuses.FirstOrDefault(s => s.CargoId == c.Id)?.Status
                     ?? RouteStatus.Assigned;
            return (Cargo: c, Status: st);
        }).ToList();

        var keyboardAll = KeyboardHelpers.BuildMainCargoKeyboard(cargoAndStatuses);

        await bot.SendMessage(
            chatId:      chatId,
            text:        "Select a cargo for detailed information or further actions:",
            replyMarkup: keyboardAll,
            cancellationToken: ct
        );
    }
}
