using CargoDesk.Application.DTOs;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Domain.Interfaces.Services;

public interface IDriverService
{
    Task<List<DriverEntity>> GetAllDrivers();
    Task<DriverEntity> GetDriverById(Guid driverId);
    Task<Guid?> GetDriverIdByDriverPhone(string phone, CancellationToken ct);
    Task<DriverEntity> CreateNewDriver(CreateNewDriverDto dto);
    Task DeleteDriverById(Guid driverId);
}