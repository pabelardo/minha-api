using Microsoft.EntityFrameworkCore;
using MyApiV8.Domain.Entities;

namespace MyApiV8.Infra.Data.Context;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<Product> Produtos { get; set; }
    public DbSet<Address> Enderecos { get; set; }
    public DbSet<Supplier> Fornecedores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
        {
            if (entry.State == EntityState.Added)
                entry.Property("CreatedAt").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified)
                entry.Property("CreatedAt").IsModified = false;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
