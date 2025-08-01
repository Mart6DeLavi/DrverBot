using System.Linq.Expressions;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly DatabaseContext _context;

    public RouteRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<RouteEntity>> GetAllRoutes()
    {
        return await _context.Routes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RouteEntity> GetRouteById(Guid routeId, CancellationToken ct)
    {
        var route = await _context.Routes.FirstOrDefaultAsync(r => r.Id.Equals(routeId), ct);

        if (route == null)
        {
            throw new KeyNotFoundException($"No route with such id: {routeId}");
        }

        return route;
    }

    public async Task<List<RouteEntity>> GetAllRoutesByDriver(Guid driverId, CancellationToken ct)
    {
        return await _context.Routes
            .AsNoTracking()
            .Where(r => r.DriverId == driverId)
            .ToListAsync(ct);
    }

    public async Task CreateNewRoute(RouteEntity entity)
    {
        await _context.Routes.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> FindSimple(Expression<Func<RouteEntity, bool>> predicate)
    {
        return await _context.Routes.AnyAsync(predicate);
    }

    public async Task DeleteRouteById(Guid routeId)
    {
        var route = await _context.Routes.FirstOrDefaultAsync(r => r.Id.Equals(routeId));

        if (route == null)
        {
            throw new KeyNotFoundException($"No route with such id:{routeId}");
        }

        _context.Routes.Remove(route);
        await _context.SaveChangesAsync();
    }
}
