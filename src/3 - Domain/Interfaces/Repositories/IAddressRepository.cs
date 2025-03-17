using MyApiV8.Domain.Entities;

namespace MyApiV8.Domain.Interfaces.Repositories;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address?> GetAddressBySupplierAsync(Guid fornecedorId);
}