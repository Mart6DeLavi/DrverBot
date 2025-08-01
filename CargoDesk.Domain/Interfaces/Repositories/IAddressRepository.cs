using System.Linq.Expressions;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Infrastructure.Persistence.Repositories;

public interface IAddressRepository
{
    Task<List<AddressEntity>> GetAllAddress();
    Task<AddressEntity> GetAddressById(Guid addressId);

    Task<bool> FindSimple(Expression<Func<AddressEntity, bool>> predicate);

    Task CreateNewRoute(AddressEntity entity);
    Task DeleteAddressById(Guid addressId);
}