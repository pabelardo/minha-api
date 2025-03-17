using AutoMapper;
using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Services;
using MyApiV8.Application.Services.Base;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Domain.Interfaces.Services;

namespace MyApiV8.Application.Services;

public class ProductAppService(
    INotifier notifier,
    INotification notification,
    IMapper mapper,
    IApplicationResponse applicationResponse,
    IProductRepository productRepository,
    IProductService productService) : BaseRepoAppService<Product, ProductDTO>(notifier, notification, productRepository, mapper, applicationResponse), IProductAppService
{
    private readonly IProductService _productService = productService;

    public override async Task<IApplicationResponse> AddAsync(ProductDTO dto, bool withApplicationResponse = true) => 
        await ApplicationResponse(await _productService.AddAsync(_mapper.Map<Product>(dto), true), StatusCodeEnum.BadRequest);

    public override async Task<IApplicationResponse> GetByIdAsync(Guid id, bool returnApplicationResponse = true) =>
        await ApplicationResponse(await _productService.GetByIdOptionAsync(id), StatusCodeEnum.NotFound);

    public override async Task<IApplicationResponse> UpdateAsync(Guid id, ProductDTO dto, bool returnApplicationResponse = true) => 
        await ApplicationResponse(await _productService.UpdateAsync(_mapper.Map<Product>(dto)), StatusCodeEnum.BadRequest);

    public override async Task<IApplicationResponse> RemoveAsync(Guid id, bool returnApplicationResponse = true)
    {
        await _productService.RemoveAsync(id);

        return await ApplicationResponse(StatusCodeEnum.NotFound);
    }

    public async Task<IApplicationResponse> GetProductSupplierAsync(Guid id) => 
        await ApplicationResponse(await _productService.GetProductSupplierAsync(id), StatusCodeEnum.NotFound);

    public async Task<IApplicationResponse> GetProductsSuppliersAsync() => 
        await _applicationResponse.Ok(_mapper.Map<IEnumerable<ProductDTO>>(await _productService.GetProductsSuppliersAsync()));

    public async Task<IApplicationResponse> GetProductsBySupplierIdAsync(Guid supplierId) =>
        await _applicationResponse.Ok(_mapper.Map<IEnumerable<ProductDTO>>(await _productService.GetProductsBySupplierIdAsync(supplierId)));
}