using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public class CargoRepository : ICargoRepository
{
    private readonly DatabaseContext _context;

    public CargoRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<CargoEntity>> GetAllCargos()
    {
        return await _context.Cargos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<CargoEntity> GetCargoById(Guid cargoId, CancellationToken ct)
    {
        var cargo = await _context.Cargos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id.Equals(cargoId));

        if (cargo == null)
        {
            throw new KeyNotFoundException($"No cargo with such id: {cargoId}");
        }

        return cargo;
    }

    public async Task<List<CargoEntity>> GetCargosByIds(IEnumerable<Guid> cargoIds, CancellationToken ct)
    {
        var ids = cargoIds.ToList();
        return await _context.Cargos
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(ct);
    }

    public async Task CreateNewCargo(CargoEntity entity)
    {
        await _context.Cargos.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async  Task DeleteCargoById(Guid cargoId)
    {
        var cargo = await _context.Cargos
            .FirstOrDefaultAsync(c => c.Id.Equals(cargoId));

        if (cargo == null)
        {
            throw new KeyNotFoundException($"No cargo with such id: {cargoId}");
        }

        _context.Cargos.Remove(cargo);
        await _context.SaveChangesAsync();
    }
}