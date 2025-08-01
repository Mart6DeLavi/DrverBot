using CargoDesk.Infrastructure.Persistence.Entities.Telegram;

namespace CargoDesk.Application.Services.Telegram;

public interface IDriverChatMappingRepository
{
    Task<DriverChatMappingEntity?> FindByDriverIdAsync(Guid driverId, CancellationToken ct);
    Task<DriverChatMappingEntity?> FindByChatIdAsync(long chatId, CancellationToken ct);
    Task AddAsync(DriverChatMappingEntity mappingEntity, CancellationToken ct);
    Task UpdateAsync(DriverChatMappingEntity mappingEntity, CancellationToken ct);
}