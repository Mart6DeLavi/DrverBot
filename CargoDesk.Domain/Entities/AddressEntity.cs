using System.ComponentModel.DataAnnotations;

namespace CargoDesk.Infrastructure.Persistence.Entities;

public class AddressEntity : BaseEntity
{
    public string CountryCode { get; set; }
    public string CompanyName { get; set; }
    public string Street { get; set; }

    [Phone]
    public string Phone { get; set; }

    public string PostCode { get; set; }
    public string City { get; set; }
    public string ContactPersonFirstName { get; set; }
    public string ContactPersonLastName { get; set; }

    [Phone]
    public string? ContactPersonPhoneNumber { get; set; }
}
