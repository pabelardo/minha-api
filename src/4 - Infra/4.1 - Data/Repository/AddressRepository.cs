using Microsoft.EntityFrameworkCore;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Infra.Data.Context;

namespace MyApiV8.Infra.Data.Repository;

public class AddressRepository(MyDbContext context) : Repository<Address>(context), IAddressRepository
{
    public Task<Address?> GetAddressBySupplierAsync(Guid fornecedorId) =>
        Db.Enderecos
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.SupplierId == fornecedorId);
}