using CargoDesk.Infrastructure.Persistence.Enums;

namespace CargoDesk.Infrastructure.Bots;

public class PendingStatusChange
{
    public Guid RouteId { get; set; }
    public Guid CargoId { get; set; }
    public RouteStatus NewStatus { get; set; }
    public CancellationTokenSource CancellationTokenSource { get; set; } = null;
}