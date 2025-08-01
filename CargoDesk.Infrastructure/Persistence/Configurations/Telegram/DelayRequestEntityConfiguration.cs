using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class DelayRequestEntityConfiguration : IEntityTypeConfiguration<DelayRequestEntity>
{
    public void Configure(EntityTypeBuilder<DelayRequestEntity> builder)
    {
        builder.ToTable("delay_requests");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DriverId).IsRequired();
        builder.Property(d => d.ChatId).IsRequired();
        builder.Property(d => d.DispatcherId).IsRequired();
        builder.Property(d => d.DelayTime)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(d => d.CreatedAt).IsRequired();
    }
}