using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApiV8.Data.Mappings.Identity;

public class IdentityRoleMapping : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.ToTable("IdentityRole");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.NormalizedName).IsUnique();
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.NormalizedName).HasMaxLength(256);
    }
}
