using System.ComponentModel.DataAnnotations;
using CargoDesk.Application.Mappings;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Application.DTOs;

public record CreateNewDriverDto(
    [Required, MaxLength(50)] string FirstName,
    [Required, MaxLength(50)] string LastName,
    [Required, MaxLength(60)] string Email,
    [Required, MaxLength(20)] string PhoneNumber
) : IMapTo<DriverEntity>;