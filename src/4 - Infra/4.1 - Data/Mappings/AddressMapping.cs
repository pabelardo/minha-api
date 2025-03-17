using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApiV8.Domain.Entities;

namespace MyApiV8.Infra.Data.Mappings;

public class AddressMapping : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(c => c.Address1)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Address2)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(c => c.Number)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(c => c.ZipCode)
            .IsRequired()
            .HasColumnType("varchar(8)");

        builder.Property(c => c.Neighborhood)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.City)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.State)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.ToTable("Addresses");
    }
}