using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using CargoDesk.Infrastructure.Persistence.Enums;

namespace CargoDesk.Application.Services.Telegram;

public interface IRouteCargoStatusRepository
{
    Task<RouteCargoStatusEntity?> GetByRouteAndCargoAsync(Guid routeId, Guid cargoId, CancellationToken ct);
    Task<List<RouteCargoStatusEntity>> GetByRouteAsync(Guid routeId, CancellationToken ct);
    Task<List<RouteCargoStatusEntity>> GetByRoutesAsync(IEnumerable<Guid> routeIds, CancellationToken ct);
    Task UpdateStatusAsync(Guid routeId, Guid cargoId, RouteStatus status, CancellationToken ct);
    Task AddAsync(RouteCargoStatusEntity entity, CancellationToken ct);
    Task AddRangeAsync(IEnumerable<RouteCargoStatusEntity> entities, CancellationToken ct);
}