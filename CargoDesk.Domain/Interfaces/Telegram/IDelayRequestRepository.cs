using CargoDesk.Infrastructure.Persistence.Entities.Telegram;

namespace CargoDesk.Application.Services.Telegram;

public interface IDelayRequestRepository
{
    Task<DelayRequestEntity> CreateAsync(DelayRequestEntity delay, CancellationToken ct);
}