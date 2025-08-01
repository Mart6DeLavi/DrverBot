using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class IssueProofEntityConfiguration : IEntityTypeConfiguration<IssueProofEntity>
{
    public void Configure(EntityTypeBuilder<IssueProofEntity> builder)
    {
        builder.ToTable("issue_proofs");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.IssueId).IsRequired();
        builder.Property(p => p.MessageType)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(p => p.Content).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
    }
}