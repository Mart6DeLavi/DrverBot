using CargoDesk.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CargoDesk.Infrastructure.Persistence.Configurations;

public class AddressEntityConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.ToTable("address");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.CountryCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(a => a.CompanyName)
            .IsRequired();

        builder.Property(a => a.Street)
            .IsRequired();

        builder.Property(a => a.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.PostCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.City)
            .IsRequired();

        builder.Property(a => a.ContactPersonFirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.ContactPersonLastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.ContactPersonPhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);
    }
}