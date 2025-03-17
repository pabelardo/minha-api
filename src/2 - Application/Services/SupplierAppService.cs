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

public class SupplierAppService(
    INotifier notifier,
    INotification notification,
    IMapper mapper,
    IApplicationResponse applicationResponse,
    ISupplierService supplierService,
    ISupplierRepository supplierRepository) : BaseRepoAppService<Supplier, SupplierDTO>(notifier, notification, supplierRepository, mapper, applicationResponse), ISupplierAppService
{
    public override async Task<IApplicationResponse> AddAsync(SupplierDTO dto, bool withApplicationResponse = true)
    {
        var supplier = await supplierService.AddAsync(_mapper.Map<Supplier>(dto), true);

        return await supplier.Match(async supplier => 
            await _applicationResponse.Ok(_mapper.Map<SupplierDTO>(supplier), "Supplier added successfully."),
            _applicationResponse.BadRequest(_notifier.GetMessages()));
    }

    public override async Task<IApplicationResponse> GetByIdAsync(Guid id, bool returnApplicationResponse = true) => 
        await ApplicationResponse(await supplierService.GetByIdOptionAsync(id), StatusCodeEnum.NotFound);

    public override async Task<IApplicationResponse> UpdateAsync(Guid id, SupplierDTO dto, bool returnApplicationResponse = true) => 
        await ApplicationResponse(await supplierService.UpdateAsync(id, _mapper.Map<Supplier>(dto)), StatusCodeEnum.NotFound);

    public override async Task<IApplicationResponse> RemoveAsync(Guid id, bool returnApplicationResponse = true) =>
        await ApplicationResponse(StatusCodeEnum.NotFound);

    public async Task<IApplicationResponse> GetSupplierAddressAsync(Guid id) => 
        await ApplicationResponse(await supplierService.GetSupplierAddressAsync(id), StatusCodeEnum.NotFound);

    public async Task<IApplicationResponse> GetSupplierProductsAddressAsync(Guid id) => 
        await ApplicationResponse(await supplierService.GetSupplierProductsAddressAsync(id), StatusCodeEnum.NotFound);
}