using LanguageExt;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Base;

namespace MyApiV8.Domain.Interfaces.Services;

public interface ISupplierService : IBaseRepoService<Supplier>
{
    Task UpdateAddressAsync(Address endereco);
    Task<Option<Supplier>> GetSupplierAddressAsync(Guid id);
    Task<Option<Supplier>> GetSupplierProductsAddressAsync(Guid id);
}