using CargoDesk.Infrastructure.Persistence.Entities.Telegram;

namespace CargoDesk.Application.Services.Telegram;

public interface IIssueRepository
{
    Task<IssueEntity> CreateAsync(IssueEntity issue, CancellationToken ct);
    Task<IssueEntity?> GetByChatIdAsync(long chatId, CancellationToken ct);
    Task<IssueProofEntity> AddProofAsync(IssueProofEntity proof, CancellationToken ct);
}