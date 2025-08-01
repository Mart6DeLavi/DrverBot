using CargoDesk.Application.Services.Telegram;
using CargoDesk.Infrastructure.Persistence.Context;
using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;

namespace CargoDesk.Infrastructure.Persistence.Repositories.Telegram;

public class IssueRepository : IIssueRepository
{
    private readonly DatabaseContext _context;

    public IssueRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IssueEntity> CreateAsync(IssueEntity issue, CancellationToken ct = default)
    {
        issue.CreatedAt = DateTime.UtcNow;
        await _context.Issues.AddAsync(issue, ct);
        await _context.SaveChangesAsync(ct);
        return issue;
    }

    public async Task<IssueEntity?> GetByChatIdAsync(long chatId, CancellationToken ct = default)
    {
        return await _context.Issues
            .Include(i => i.Proofs)
            .FirstOrDefaultAsync(i => i.ChatId == chatId, ct);
    }

    public async Task<IssueProofEntity> AddProofAsync(IssueProofEntity proof, CancellationToken ct = default)
    {
        proof.CreatedAt = DateTime.UtcNow;
        await _context.IssueProofs.AddAsync(proof, ct);
        await _context.SaveChangesAsync(ct);
        return proof;
    }
}