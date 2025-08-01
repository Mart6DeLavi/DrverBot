using CargoDesk.Application.DTOs;
using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Enums;

namespace CargoDesk.Domain.Interfaces.Services;

public interface IRouteService
{
    Task<List<RouteEntity>> GetAllRoutes();
    Task<RouteEntity> GetRouteById(Guid routeId, CancellationToken ct);

    Task<List<RouteEntity>> GetAllRoutesByDriver(Guid driverId, CancellationToken ct);

    Task<RouteEntity> CreateNewRoute(CreateNewRouteDto dto, CancellationToken ct);

    Task ChangeCargoStatusAsync(Guid routeId, Guid cargoId, RouteStatus newStatus, CancellationToken ct);

    Task DeleteRouteById(Guid routeId);
}
