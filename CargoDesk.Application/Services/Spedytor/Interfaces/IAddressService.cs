using CargoDesk.Application.DTOs;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Domain.Interfaces.Services;

public interface IAddressService
{
    Task<List<AddressEntity>> GetAllAddresses();
    Task<AddressEntity> GetAddressById(Guid addressId);
    Task<AddressEntity> CreateNewAddress(CreateNewAddressDto dto);
    Task DeleteAddressById(Guid addressId);
}