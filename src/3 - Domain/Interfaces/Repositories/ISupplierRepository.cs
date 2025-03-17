using MyApiV8.Domain.Entities;

namespace MyApiV8.Domain.Interfaces.Repositories;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<Supplier?> GetSupplierAddressAsync(Guid id);
    Task<Supplier?> GetSupplierProductsAddressAsync(Guid id);
}