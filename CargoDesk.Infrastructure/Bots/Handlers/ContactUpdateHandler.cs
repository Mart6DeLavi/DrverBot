using CargoDesk.Application.Services.Telegram;
using CargoDesk.Domain.Interfaces.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CargoDesk.Infrastructure.Bots;

public class ContactUpdateHandler : IUpdateHandler
{
    private readonly IDriverService _driverService;
    private readonly IDriverChatMappingService _driverChatMappingService;

    public ContactUpdateHandler(IDriverService driverService, IDriverChatMappingService driverChatMappingService)
    {
        _driverService = driverService;
        _driverChatMappingService = driverChatMappingService;
    }

    public async Task<bool> CanHandleAsync(Update update, CancellationToken ct)
    {
        return await Task.FromResult(
            update.Type == UpdateType.Message
            && update.Message?.Contact is not null
        );
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var contact = update.Message!.Contact!;
        var rawPhone = contact.PhoneNumber;
        var normalized = new string(rawPhone.Where(char.IsDigit).ToArray());
        var chatId = update.Message.Chat.Id;
        var driver = await _driverService.GetDriverIdByDriverPhone(normalized, ct);

        if (!driver.HasValue)
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "We can't find your number in system",
                cancellationToken: ct
            );
            return;
        }

        await _driverChatMappingService.MapAsync(
            driverId: driver.Value,
            phoneNumber: normalized,
            chatId: chatId,
            ct
        );

        await bot.SendMessage(
            chatId: chatId,
            text: "Thanks. We have registered you.",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: ct
        );
    }
}