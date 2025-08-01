using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public interface ICargoRepository
{
    Task<List<CargoEntity>> GetAllCargos();
    Task<CargoEntity> GetCargoById(Guid cargoId, CancellationToken ct);
    Task<List<CargoEntity>> GetCargosByIds(IEnumerable<Guid> cargoIds, CancellationToken ct);

    Task CreateNewCargo(CargoEntity entity);
    Task DeleteCargoById(Guid cargoId);

}