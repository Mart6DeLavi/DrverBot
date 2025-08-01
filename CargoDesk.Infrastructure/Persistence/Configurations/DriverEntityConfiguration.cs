using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class DriverEntityConfiguration : IEntityTypeConfiguration<DriverEntity>
{
    public void Configure(EntityTypeBuilder<DriverEntity> builder)
    {
        builder.ToTable("drivers");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .ValueGeneratedOnAdd();

        builder.Property(d => d.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.Email)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(d => d.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany<RouteEntity>()
            .WithOne()
            .HasForeignKey(r => r.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}