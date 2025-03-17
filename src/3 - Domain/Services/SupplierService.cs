using LanguageExt;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Domain.Interfaces.Services;
using MyApiV8.Domain.Services.Base;

namespace MyApiV8.Domain.Services;

public class SupplierService(
    INotifier notifier,
    INotification notification,
    ISupplierRepository supplierRepository,
    IAddressRepository addressRepository) : BaseRepoService<Supplier>(supplierRepository, notifier, notification), ISupplierService
{
    private const string SupplierNotFound = "The supplier was not found.";

    public override async Task AddAsync(Supplier supplier)
    {
        if (!await Validate(supplier) || !await Validate(supplier.Address)) return;

        if (supplierRepository.GetByFilterAsync(f => f.Document == supplier.Document).Result.Any())
        {
            Notify("There is an already supplier with this document informed.");
            return;
        }

        await supplierRepository.AddAsync(supplier);
    }

    public override async Task<Option<Supplier>> UpdateAsync(Supplier supplier)
    {
        if(!await Validate(supplier)) return Option<Supplier>.None;

        return await supplierRepository.UpdateAsync(supplier);
    }


    public async Task UpdateAddressAsync(Address address)
    {
        if (!await Validate(address)) return;

        await addressRepository.UpdateAsync(address);
    }

    public async Task<Option<Supplier>> GetSupplierAddressAsync(Guid id)
    {
        var supplier = await supplierRepository.GetSupplierAddressAsync(id);

        if (supplier is null)
        {
            Notify(SupplierNotFound);
            return Option<Supplier>.None;
        }

        return supplier;
    }

    public async Task<Option<Supplier>> GetSupplierProductsAddressAsync(Guid id)
    {
        var supplier = await supplierRepository.GetSupplierProductsAddressAsync(id);

        if (supplier is null)
        {
            Notify(SupplierNotFound);
            return Option<Supplier>.None;
        }

        return supplier;
    }
}