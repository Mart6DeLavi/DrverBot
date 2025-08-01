namespace CargoDesk.Infrastructure.Persistence.Entities.Telegram;

public class IssueProofEntity
{
    public Guid Id { get; set; }
    public Guid IssueId { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public IssueEntity Issue { get; set; } = null!;
}