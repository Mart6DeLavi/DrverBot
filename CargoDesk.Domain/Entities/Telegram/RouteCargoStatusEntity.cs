using CargoDesk.Infrastructure.Persistence.Enums;

namespace CargoDesk.Infrastructure.Persistence.Entities.Telegram;

public class RouteCargoStatusEntity : BaseEntity
{
    public Guid RouteId { get; set; }
    public Guid CargoId { get; set; }
    public RouteStatus Status { get; set; }
}