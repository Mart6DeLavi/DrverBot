namespace CargoDesk.Infrastructure.Persistence.Entities.Telegram;

public class DriverWorkSessionEntity
{
    public Guid DriverId { get; set; }
    public DateTime? WorkStartAt { get; set; }
    public DateTime? WorkEndAt { get; set; }
    public Guid RouteId { get; set; }
}