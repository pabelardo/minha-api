using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyApiV8.Data.Mappings.Identity;

public class IdentityUserRoleMapping : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.ToTable("IdentityUserRole");
        builder.HasKey(r => new { r.UserId, r.RoleId });
    }
}
