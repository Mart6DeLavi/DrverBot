using System.Linq.Expressions;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly DatabaseContext _context;

    public DriverRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<DriverEntity>> GetAllDrivers()
    {
        return await _context.Drivers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DriverEntity> GetDriverById(Guid driverId)
    {
        var driver = await _context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id.Equals(driverId));

        if (driver == null)
        {
            throw new KeyNotFoundException($"No driver with such id: {driverId}");
        }

        return driver;
    }

    public async Task<Guid?> GetDriverIdByDriverPhone(string phone, CancellationToken ct)
    {
        var driver = await _context.Drivers
            .FirstOrDefaultAsync(d => d.PhoneNumber.Equals(phone), ct);

        if (driver is null)
        {
            throw new KeyNotFoundException($"No driver with such phone: {phone}");
        }

        return driver.Id;
    }

    public async Task<bool> FindSimple(Expression<Func<DriverEntity, bool>> predicate) =>
        await _context.Drivers.AnyAsync(predicate);

    public async Task CreateNewDriver(DriverEntity driver)
    {
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDriverById(Guid driverId)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id.Equals(driverId));

        if (driver == null)
        {
            throw new KeyNotFoundException($"No driver with such id: {driverId}");
        }

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();
    }
}