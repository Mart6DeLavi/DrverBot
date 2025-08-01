using CargoDesk.Application.Mappings;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Application.DTOs;

public record CreateNewRouteDto(
    Guid DriverId,
    List<Guid> CargoIds
) : IMapTo<RouteEntity>;
