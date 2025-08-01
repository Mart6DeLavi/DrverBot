using CargoDesk.Infrastructure.Persistence.Entities.Telegram;

namespace CargoDesk.Application.Services.Telegram;

public interface IDriverWorkSessionRepository
{
    Task<DriverWorkSessionEntity?> GetActiveSessionAsync(Guid driverId, CancellationToken ct);
    Task StartWorkAsync(Guid driverId, Guid routeId, CancellationToken ct);
    Task<DriverWorkSessionEntity?> FinishWorkAsync(Guid driverId, CancellationToken ct);
    Task<DriverWorkSessionEntity?> GetLastSessionAsync(Guid driverId, CancellationToken ct);
}