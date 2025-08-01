using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class IssueEntityConfiguration : IEntityTypeConfiguration<IssueEntity>
{
    public void Configure(EntityTypeBuilder<IssueEntity> builder)
    {
        builder.ToTable("issues");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.DriverId).IsRequired();
        builder.Property(i => i.ChatId).IsRequired();
        builder.Property(i => i.IssueType)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(i => i.DispatcherId).IsRequired();
        builder.Property(i => i.CreatedAt).IsRequired();

        builder.HasMany(i => i.Proofs)
            .WithOne(p => p.Issue)
            .HasForeignKey(p => p.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.ChatId);
        builder.HasIndex(i => i.DispatcherId);
    }
}