using MyApiV8.Domain.Entities;

namespace MyApiV8.Domain.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(Guid supplierId);
    Task<IEnumerable<Product>> GetProductsSuppliersAsync();
    Task<Product?> GetProductSupplierAsync(Guid id);
}