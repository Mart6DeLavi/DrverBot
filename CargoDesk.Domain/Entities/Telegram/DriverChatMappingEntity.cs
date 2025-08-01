using System.ComponentModel.DataAnnotations;

namespace CargoDesk.Infrastructure.Persistence.Entities.Telegram;

public class DriverChatMappingEntity
{
    [Key]
    public Guid DriverId { get; set; }

    public string DriverPhone { get; set; }
    public long ChatId { get; set; }
}