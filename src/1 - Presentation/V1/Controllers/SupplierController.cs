using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Services;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.V1.Controllers.Base;
using static MyApiV8.Extensions.CustomAuthorization;

namespace MyApiV8.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/suppliers")]
public class SupplierController(
    INotifier notifier,
    INotification notification,
    IUser appUser,
    IApplicationResponse applicationResponse,
    IMapper mapper,
    ISupplierAppService supplierAppService) : MainController(notifier, notification, appUser, applicationResponse, mapper)
{
    #region Routes

    [ClaimsAuthorize("Supplier", "Add")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Add(SupplierDTO supplierDTO)
    {
        var supplier = await supplierAppService.AddAsync(supplierDTO, true);

        return CustomResponse(supplier);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAll()
    {
        var suppliers = await supplierAppService.GetAllAsync(true);

        return CustomResponse(suppliers);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetById(Guid id)
    {
        var supplier = await supplierAppService.GetByIdAsync(id, true);

        return CustomResponse(supplier);
    }

    [ClaimsAuthorize("Supplier", "Update")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, SupplierDTO dto)
    {
        var supplier = await supplierAppService.UpdateAsync(id, dto);

        return CustomResponse(supplier);
    }

    [ClaimsAuthorize("Supplier", "Delete")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Remove(Guid id)
    {
        var supplier = await supplierAppService.RemoveAsync(id, true);

        return CustomResponse(supplier);
    }

    [HttpGet("get-address/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetSupplierAddress(Guid id)
    {
        var supplier = await supplierAppService.GetSupplierAddressAsync(id);

        return CustomResponse(supplier);
    }

    [HttpGet("get-products-address/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetSupplierProductsAddress(Guid id)
    {
        var supplier = await supplierAppService.GetSupplierProductsAddressAsync(id);

        return CustomResponse(supplier);
    }

    #endregion
}
