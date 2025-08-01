using CargoDesk.Application.DTOs;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Domain.Interfaces.Services;

public interface ICargoService
{
    Task<List<CargoEntity>> GetAllCargos();
    Task<CargoEntity> GetCargoById(Guid cargoId, CancellationToken ct);
    Task<CargoEntity> CreateNewCargo(CreateNewCargoDto dto);
    Task DeleteCargoById(Guid cargoId);
}