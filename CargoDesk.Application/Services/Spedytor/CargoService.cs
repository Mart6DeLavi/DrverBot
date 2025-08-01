using AutoMapper;
using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Repositories;

namespace CargoDesk.Application.Services;

public class CargoService : ICargoService
{
    private readonly ICargoRepository _cargoRepository;
    private readonly IMapper _mapper;

    public CargoService(ICargoRepository cargoRepository, IMapper mapper)
    {
        _cargoRepository = cargoRepository;
        _mapper = mapper;
    }

    public async  Task<List<CargoEntity>> GetAllCargos()
    {
        return await _cargoRepository.GetAllCargos();
    }

    public async Task<CargoEntity> GetCargoById(Guid cargoId, CancellationToken ct)
    {
        return await _cargoRepository.GetCargoById(cargoId, ct);
    }

    public async Task<CargoEntity> CreateNewCargo(CreateNewCargoDto dto)
    {
        await _cargoRepository.CreateNewCargo(_mapper.Map<CargoEntity>(dto));
        return _mapper.Map<CargoEntity>(dto);
    }

    public async  Task DeleteCargoById(Guid cargoId)
    {
        await _cargoRepository.DeleteCargoById(cargoId);
    }
}