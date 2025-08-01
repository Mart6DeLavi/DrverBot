using System.ComponentModel.DataAnnotations;
using CargoDesk.Application.Mappings;
using CargoDesk.Infrastructure.Persistence.Entities;

namespace CargoDesk.Application.DTOs;

public record CreateNewAddressDto(
    [Required, MaxLength(3)] string CountryCode,
    [Required] string CompanyName,
    [Required] string Street,
    [Required] string Phone,
    [Required, MaxLength(50)] string PostCode,
    [Required] string City,
    [Required, MaxLength(50)] string ContactPersonFirstName,
    [Required, MaxLength(50)] string ContactPersonLastName,
    string ContactPersonPhoneNumber
) : IMapTo<AddressEntity>;