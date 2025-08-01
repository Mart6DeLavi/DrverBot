using AutoMapper;
using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Repositories;

namespace CargoDesk.Application.Services;

public class DriverService : IDriverService
{
    private readonly IMapper _mapper;
    private readonly IDriverRepository _driverRepository;

    public DriverService(IMapper mapper, IDriverRepository driverRepository)
    {
        _mapper = mapper;
        _driverRepository = driverRepository;
    }

    public async Task<List<DriverEntity>> GetAllDrivers()
    {
        return await _driverRepository.GetAllDrivers();
    }

    public async Task<DriverEntity> GetDriverById(Guid driverId)
    {
        return await _driverRepository.GetDriverById(driverId);
    }

    public async Task<Guid?> GetDriverIdByDriverPhone(string phone, CancellationToken ct)
    {
        return await _driverRepository.GetDriverIdByDriverPhone(phone, ct);
    }

    public async Task<DriverEntity> CreateNewDriver(CreateNewDriverDto dto)
    {
        var exists = await _driverRepository.FindSimple(d =>
            d.FirstName == dto.FirstName &&
            d.LastName == dto.LastName &&
            d.Email == dto.Email &&
            d.PhoneNumber == dto.PhoneNumber
        );

        if (exists)
        {
            throw new InvalidDataException($"driver already exists.");
        }

        var entity = _mapper.Map<DriverEntity>(dto);
        await _driverRepository.CreateNewDriver(entity);
        return entity;
    }

    public async Task DeleteDriverById(Guid driverId)
    {
        await _driverRepository.DeleteDriverById(driverId);
    }
}