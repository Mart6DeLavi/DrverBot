using AutoMapper;
using CargoDesk.Application.DTOs;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Repositories;

namespace CargoDesk.Application.Services;

public class AddressService : IAddressService
{
    private readonly IMapper _mapper;
    private readonly IAddressRepository _addressRepository;

    public AddressService(IMapper mapper, IAddressRepository addressRepository)
    {
        _mapper = mapper;
        _addressRepository = addressRepository;
    }

    public async Task<List<AddressEntity>> GetAllAddresses()
    {
        return await _addressRepository.GetAllAddress();
    }

    public async Task<AddressEntity> GetAddressById(Guid addressId)
    {
        return await _addressRepository.GetAddressById(addressId);
    }

    public async Task<AddressEntity> CreateNewAddress(CreateNewAddressDto dto)
    {
        var exists = await _addressRepository.FindSimple(a =>
            a.CountryCode == dto.CountryCode &&
            a.CompanyName == dto.CompanyName &&
            a.Street == dto.Street &&
            a.PostCode == dto.PostCode &&
            a.City == dto.City
        );

        if (exists)
        {
            throw new InvalidDataException($"address already exists");
        }

        var entity = _mapper.Map<AddressEntity>(dto);
        await _addressRepository.CreateNewRoute(entity);
        return entity;
    }

    public async Task DeleteAddressById(Guid addressId)
    {
        await _addressRepository.DeleteAddressById(addressId);
    }
}