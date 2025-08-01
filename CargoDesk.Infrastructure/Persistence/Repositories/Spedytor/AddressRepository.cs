using System.Linq.Expressions;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly DatabaseContext _context;

    public AddressRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<AddressEntity>> GetAllAddress()
    {
        return await _context.Addresses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<AddressEntity> GetAddressById(Guid addressId)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id.Equals(addressId));

        if (address == null)
        {
            throw new KeyNotFoundException($"No address with such id: {addressId}");
        }

        return address;
    }

    public async Task<bool> FindSimple(Expression<Func<AddressEntity, bool>> predicate) =>
        await _context.Addresses.AnyAsync(predicate);

    public async Task CreateNewRoute(AddressEntity entity)
    {
        await _context.Addresses
            .AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAddressById(Guid addressId)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id.Equals(addressId));

        if (address == null)
        {
            throw new KeyNotFoundException($"No address with such id: {addressId}");
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
    }
}