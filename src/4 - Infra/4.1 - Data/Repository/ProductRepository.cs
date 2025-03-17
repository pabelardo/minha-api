using Microsoft.EntityFrameworkCore;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Infra.Data.Context;

namespace MyApiV8.Infra.Data.Repository;

public class ProductRepository(MyDbContext context) : Repository<Product>(context), IProductRepository
{
    public Task<Product?> GetProductSupplierAsync(Guid id) =>
        Db.Produtos
            .AsNoTracking()
            .Include(f => f.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Product>> GetProductsSuppliersAsync() =>
        await Db
            .Produtos
            .AsNoTracking()
            .Include(f => f.Supplier)
            .OrderBy(p => p.Name)
            .ToListAsync();

    public Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(Guid supplierId) =>
        GetByFilterAsync(p => p.SupplierId == supplierId);
}