using AutoMapper;
using CargoDesk.Application.DTOs;
using CargoDesk.Application.Services.Telegram;
using CargoDesk.Domain.Interfaces.Services;
using CargoDesk.Infrastructure.Persistence.Entities;
using CargoDesk.Infrastructure.Persistence.Enums;
using CargoDesk.Infrastructure.Persistence.Repositories;

namespace CargoDesk.Application.Services;

public class RouteService : IRouteService
{
    private readonly IMapper _mapper;
    private readonly IRouteRepository _routeRepository;
    private readonly IRouteNotificationService _notifier;
    private readonly IRouteCargoStatusRepository _statusRepository;

    public RouteService(
        IMapper mapper,
        IRouteRepository routeRepository,
        IRouteNotificationService notifier,
        IRouteCargoStatusRepository statusRepository)
    {
        _mapper = mapper;
        _routeRepository = routeRepository;
        _notifier = notifier;
        _statusRepository = statusRepository;
    }

    public async Task<List<RouteEntity>> GetAllRoutes()
    {
        return await _routeRepository.GetAllRoutes();
    }

    public async Task<RouteEntity> GetRouteById(Guid routeId, CancellationToken ct)
    {
        return await _routeRepository.GetRouteById(routeId, ct);
    }

    public async Task<List<RouteEntity>> GetAllRoutesByDriver(Guid driverId, CancellationToken ct)
    {
        return await _routeRepository.GetAllRoutesByDriver(driverId, ct);
    }

    public async Task<RouteEntity> CreateNewRoute(CreateNewRouteDto dto, CancellationToken ct)
    {
        var exists = await _routeRepository.FindSimple(r =>
            r.DriverId == dto.DriverId
        );

        if (exists)
        {
            throw new InvalidDataException("route already exists");
        }

        var entity = _mapper.Map<RouteEntity>(dto);
        await _routeRepository.CreateNewRoute(entity);

        await _notifier.NotifyDriverRouteCreatedAsync(
            driverId: entity.DriverId,
            routeId: entity.Id,
            ct
        );

        return entity;
    }

    public async Task ChangeCargoStatusAsync(Guid routeId, Guid cargoId, RouteStatus newStatus, CancellationToken ct)
    {
        await _statusRepository.UpdateStatusAsync(routeId, cargoId, newStatus, ct);
    }

    public async Task DeleteRouteById(Guid routeId)
    {
        await _routeRepository.DeleteRouteById(routeId);
    }
}