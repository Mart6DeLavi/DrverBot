using System.Linq.Expressions;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public interface IDriverRepository
{
    Task<List<DriverEntity>> GetAllDrivers();
    Task<DriverEntity> GetDriverById(Guid driverId);
    Task<Guid?> GetDriverIdByDriverPhone(string phone, CancellationToken ct);

    Task<bool> FindSimple(Expression<Func<DriverEntity, bool>> predicate);

    Task CreateNewDriver(DriverEntity driver);
    Task DeleteDriverById(Guid driverId);
}