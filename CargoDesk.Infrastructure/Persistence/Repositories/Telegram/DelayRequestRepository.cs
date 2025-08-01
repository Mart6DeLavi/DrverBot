using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;

namespace CargoDesk.Infrastructure.Persistence.Repositories.Telegram;

public class DelayRequestRepository : IDelayRequestRepository
{
    private readonly DatabaseContext _context;

    public DelayRequestRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<DelayRequestEntity> CreateAsync(DelayRequestEntity delay, CancellationToken ct)
    {
        delay.CreatedAt = DateTime.UtcNow;
        await _context.DelayRequest.AddAsync(delay, ct);
        await _context.SaveChangesAsync(ct);
        return delay;
    }
}