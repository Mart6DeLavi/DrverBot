namespace CargoDesk.Infrastructure.Persistence.Entities.Telegram;

public class IssueEntity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public long ChatId { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public Guid DispatcherId { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<IssueProofEntity> Proofs { get; set; } = new List<IssueProofEntity>();
}