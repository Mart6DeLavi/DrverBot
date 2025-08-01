using CargoDesk.Infrastructure.Persistence.Entities.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class RouteCargoStatusEntityConfiguration : IEntityTypeConfiguration<RouteCargoStatusEntity>
{
    public void Configure(EntityTypeBuilder<RouteCargoStatusEntity> builder)
    {
        builder.ToTable("route_cargo_statuses");
        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.RouteId).IsRequired();
        builder.Property(rc => rc.CargoId).IsRequired();
        builder.Property(rc => rc.Status).IsRequired();

        builder.HasIndex(rc => new { rc.RouteId, rc.CargoId }).IsUnique();
    }
}