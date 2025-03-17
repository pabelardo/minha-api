using LanguageExt;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Domain.Interfaces.Services;
using MyApiV8.Domain.Services.Base;

namespace MyApiV8.Domain.Services;

public class ProductService(
    INotifier notifier,
    INotification notification,
    IProductRepository produtoRepository) : BaseRepoService<Product>(produtoRepository, notifier, notification), IProductService
{
    public Task<IEnumerable<Product>> GetProductsBySupplierIdAsync(Guid supplierId) => 
        produtoRepository.GetProductsBySupplierIdAsync(supplierId);

    public Task<IEnumerable<Product>> GetProductsSuppliersAsync() =>
        produtoRepository.GetProductsSuppliersAsync();

    public async Task<Option<Product>> GetProductSupplierAsync(Guid id)
    {
        var product = await produtoRepository.GetProductSupplierAsync(id);

        await ValidateProduct(product);

        return product;
    }

    public async Task<bool> ValidateProduct(Product product)
    {
        if (product == null)
        {
            Notify("Product not found");
            return false;
        }

        if (!await Validate(product))
            return false;

        return true;
    }
}