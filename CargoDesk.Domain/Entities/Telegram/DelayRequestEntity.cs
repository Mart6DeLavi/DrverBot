namespace CargoDesk.Infrastructure.Persistence.Entities.Telegram;

public class DelayRequestEntity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public long ChatId { get; set; }
    public Guid DispatcherId { get; set; }
    public string DelayTime { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}