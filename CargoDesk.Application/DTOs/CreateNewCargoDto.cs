using System.ComponentModel.DataAnnotations;
using CargoDesk.Application.Mappings;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Application.DTOs;

public record CreateNewCargoDto(
    [Required] string ReferenceNumber,
    DateTime PickUpDateTime,
    DateTime DeliveryDateTime,
    DateTime PlannedPickUpDateTime,
    DateTime PlannedDeliveryDateTime,

    Guid PickUpAddressId,
    Guid DeliveryAddressId,

    int NumberOfPallets,
    double Weight
): IMapTo<CargoEntity>;