using LanguageExt;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Base;

namespace MyApiV8.Domain.Interfaces.Services;

public interface IProductService : IBaseRepoService<Product> 
{
    Task<Option<Product>> GetProductSupplierAsync(Guid id);
    Task<IEnumerable<Product>> GetProductsSuppliersAsync();
    Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(Guid supplierId);
}