using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class DriverWorkSessionEntityConfiguration : IEntityTypeConfiguration<DriverWorkSessionEntity>
{
    public void Configure(EntityTypeBuilder<DriverWorkSessionEntity> builder)
    {
        builder.ToTable("driver_work_session");
        builder.HasKey(e => e.DriverId);
        builder.Property(e => e.WorkStartAt);
        builder.Property(e => e.WorkEndAt);
        builder.Property(e => e.RouteId).IsRequired();
    }
}