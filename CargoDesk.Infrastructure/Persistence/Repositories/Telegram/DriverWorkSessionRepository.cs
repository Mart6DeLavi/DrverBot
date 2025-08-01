using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories.Telegram;

public class DriverWorkSessionRepository : IDriverWorkSessionRepository
{
    private readonly DatabaseContext _context;

    public DriverWorkSessionRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<DriverWorkSessionEntity?> GetActiveSessionAsync(Guid driverId, CancellationToken ct)
    {
        return await _context.DriverWorkSession.FindAsync(new object[] { driverId }, ct);
    }

    public async Task StartWorkAsync(Guid driverId, Guid routeId, CancellationToken ct)
    {
        var entry = await _context.DriverWorkSession.FindAsync(new object[] { driverId }, ct);
        if (entry == null)
        {
            entry = new DriverWorkSessionEntity
            {
                DriverId = driverId,
                RouteId = routeId,
                WorkStartAt = DateTime.UtcNow,
                WorkEndAt = null
            };
            _context.DriverWorkSession.Add(entry);
        }
        else
        {
            entry.RouteId = routeId;
            entry.WorkStartAt = DateTime.UtcNow;
            entry.WorkEndAt = null;
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task<DriverWorkSessionEntity?> FinishWorkAsync(Guid driverId, CancellationToken ct)
    {
        var session = await GetActiveSessionAsync(driverId, ct);
        if (session == null) return null;
        session.WorkEndAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return session;
    }

    public async Task<DriverWorkSessionEntity?> GetLastSessionAsync(Guid driverId, CancellationToken ct)
    {
        return await _context.DriverWorkSession
            .Where(s => s.DriverId == driverId)
            .OrderByDescending(s => s.WorkStartAt)
            .FirstOrDefaultAsync(ct);
    }
}