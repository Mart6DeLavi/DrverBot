namespace CargoDesk.Application.Services.Telegram;

public interface IDriverChatMappingService
{
    Task MapAsync(Guid driverId, string phoneNumber, long chatId, CancellationToken ct);
    Task<long?> GetChatIdByDriverIdAsync(Guid driverId, CancellationToken ct);
    Task<Guid?> GetDriverIdByChatIdAsync(long chatId, CancellationToken ct);
}