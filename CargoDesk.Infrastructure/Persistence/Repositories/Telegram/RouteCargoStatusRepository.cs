using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using CargoDesk.Infrastructure.Persistence.Enums;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories.Telegram;

public class RouteCargoStatusRepository : IRouteCargoStatusRepository
{
    private readonly DatabaseContext _context;

    public RouteCargoStatusRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<RouteCargoStatusEntity?> GetByRouteAndCargoAsync(Guid routeId, Guid cargoId, CancellationToken ct)
    {
        return await _context.RouteCargoStatus
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RouteId == routeId && x.CargoId == cargoId, ct);
    }

    public async Task<List<RouteCargoStatusEntity>> GetByRouteAsync(Guid routeId, CancellationToken ct)
    {
        return await _context.RouteCargoStatus
            .AsNoTracking()
            .Where(x => x.RouteId == routeId)
            .ToListAsync(ct);
    }

    public async Task<List<RouteCargoStatusEntity>> GetByRoutesAsync(IEnumerable<Guid> routeIds, CancellationToken ct)
    {
        var ids = routeIds.ToList();
        return await _context.RouteCargoStatus
            .AsNoTracking()
            .Where(x => ids.Contains(x.RouteId))
            .ToListAsync(ct);
    }

    public async Task UpdateStatusAsync(Guid routeId, Guid cargoId, RouteStatus status, CancellationToken ct)
    {
        var entity = await _context.RouteCargoStatus
            .FirstOrDefaultAsync(x => x.RouteId == routeId && x.CargoId == cargoId, ct);

        if (entity == null)
        {
            entity = new RouteCargoStatusEntity
            {
                Id        = Guid.NewGuid(),
                RouteId   = routeId,
                CargoId   = cargoId,
                Status    = status,
                CreatedAt = DateTime.UtcNow
            };
            _context.RouteCargoStatus.Add(entity);
        }
        else
        {
            entity.Status    = status;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(ct);
    }


    public async Task AddAsync(RouteCargoStatusEntity entity, CancellationToken ct)
    {
        await _context.RouteCargoStatus.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddRangeAsync(IEnumerable<RouteCargoStatusEntity> entities, CancellationToken ct)
    {
        await _context.RouteCargoStatus.AddRangeAsync(entities, ct);
        await _context.SaveChangesAsync(ct);
    }
}