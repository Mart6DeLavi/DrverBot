namespace CargoDesk.Infrastructure.Persistence.Entities;

public class RouteEntity : BaseEntity
{
    public Guid DriverId { get; set; }
    public List<Guid> CargoIds { get; set; } = new();
}