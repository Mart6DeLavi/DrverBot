using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class CargoEntityConfiguration : IEntityTypeConfiguration<CargoEntity>
{
    public void Configure(EntityTypeBuilder<CargoEntity> builder)
    {
        builder.ToTable("cargos");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ReferenceNumber)
            .IsRequired();

        builder.Property(c => c.PickUpDateTime)
            .IsRequired();

        builder.Property(c => c.DeliveryDateTime)
            .IsRequired();

        builder.Property(c => c.PlannedPickUpDateTime)
            .IsRequired();

        builder.Property(c => c.PlannedDeliveryDateTime)
            .IsRequired();

        builder.Property(c => c.PickUpAddressId)
            .IsRequired();

        builder.Property(c => c.DeliveryAddressId)
            .IsRequired();

        builder.Property(c => c.Weight)
            .IsRequired();

        builder.Property(c => c.NumberOfPallets)
            .IsRequired();

        builder.HasIndex(c => c.ReferenceNumber);

        builder.HasOne<AddressEntity>()
            .WithMany()
            .HasForeignKey(c => c.PickUpAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AddressEntity>()
            .WithMany()
            .HasForeignKey(c => c.DeliveryAddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
