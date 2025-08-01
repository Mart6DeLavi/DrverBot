using System.ComponentModel.DataAnnotations;

namespace CargoDesk.Infrastructure.Persistence.Entities;

public class DriverEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    [Phone] public string PhoneNumber { get; set; }
}
