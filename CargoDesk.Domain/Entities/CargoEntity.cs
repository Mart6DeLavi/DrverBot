namespace CargoDesk.Infrastructure.Persistence.Entities;

public class CargoEntity : BaseEntity
{
    public string ReferenceNumber { get; set; }
    public DateTime PickUpDateTime { get; set; }
    public DateTime DeliveryDateTime { get; set; }
    public DateTime PlannedPickUpDateTime { get; set; }
    public DateTime PlannedDeliveryDateTime { get; set; }

    public Guid PickUpAddressId { get; set; }
    public Guid DeliveryAddressId { get; set; }

    public double Weight { get; set; }
    public int NumberOfPallets { get; set; }
}