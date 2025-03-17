using Microsoft.EntityFrameworkCore;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Infra.Data.Context;

namespace MyApiV8.Infra.Data.Repository;

public class SupplierRepository(MyDbContext context) : Repository<Supplier>(context), ISupplierRepository
{
    public Task<Supplier?> GetSupplierAddressAsync(Guid id) =>
        Db.Fornecedores
            .AsNoTracking()
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<Supplier?> GetSupplierProductsAddressAsync(Guid id) =>
        Db.Fornecedores
            .AsNoTracking()
            .Include(c => c.Products)
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id);
}