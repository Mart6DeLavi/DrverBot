using System.Linq.Expressions;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public interface IRouteRepository
{
    Task<List<RouteEntity>> GetAllRoutes();
    Task<RouteEntity> GetRouteById(Guid routeId, CancellationToken ct);
    Task<List<RouteEntity>> GetAllRoutesByDriver(Guid driverId, CancellationToken ct);
    Task<bool> FindSimple(Expression<Func<RouteEntity, bool>> predicate);
    Task CreateNewRoute(RouteEntity entity);
    Task DeleteRouteById(Guid routeId);
}
