using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class DriverChatEntityConfiguration : IEntityTypeConfiguration<DriverChatMappingEntity>
{
    public void Configure(EntityTypeBuilder<DriverChatMappingEntity> builder)
    {
        builder.ToTable("drivers_chats");

        builder.HasKey(dc => dc.DriverId);

        builder.Property(dc => dc.DriverId)
            .ValueGeneratedNever();

        builder.Property(dc => dc.DriverId)
            .IsRequired();

        builder.Property(dc => dc.DriverPhone)
            .IsRequired();

        builder.Property(dc => dc.ChatId)
            .IsRequired();
    }
}