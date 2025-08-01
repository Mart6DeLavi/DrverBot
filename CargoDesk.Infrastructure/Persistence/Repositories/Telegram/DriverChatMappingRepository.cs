using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories.Telegram;

public class DriverChatMappingRepository : IDriverChatMappingRepository
{
    private readonly DatabaseContext _context;

    public DriverChatMappingRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<DriverChatMappingEntity?> FindByDriverIdAsync(Guid driverId, CancellationToken ct)
    {
        var ent = await _context.DriverChat
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DriverId.Equals(driverId), ct);

        if (ent is null)
        {
            return null;
        }

        return new DriverChatMappingEntity
        {
            DriverId = ent.DriverId,
            DriverPhone = ent.DriverPhone,
            ChatId = ent.ChatId
        };
    }

    public async Task<DriverChatMappingEntity?> FindByChatIdAsync(long chatId, CancellationToken ct)
    {
        return await _context.DriverChat
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ChatId == chatId, ct);
    }

    public async  Task AddAsync(DriverChatMappingEntity mappingEntity, CancellationToken ct)
    {
        var ent = new CargoDesk.Infrastructure.Persistence.Entities.Telegram.DriverChatMappingEntity
        {
            DriverId = mappingEntity.DriverId,
            DriverPhone = mappingEntity.DriverPhone,
            ChatId = mappingEntity.ChatId
        };
        await _context.AddAsync(ent, ct);
        await _context.SaveChangesAsync(ct);

    }

    public async Task UpdateAsync(DriverChatMappingEntity mappingEntity, CancellationToken ct)
    {
        var ent = await _context.DriverChat
            .FirstAsync(x => x.DriverId.Equals(mappingEntity.DriverId), ct);

        ent.DriverPhone = mappingEntity.DriverPhone;
        ent.ChatId = mappingEntity.ChatId;
        await _context.SaveChangesAsync(ct);
    }
}