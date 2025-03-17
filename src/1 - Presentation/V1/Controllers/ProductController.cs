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

namespace MyApiV8.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductController(
    INotifier notifier,
    INotification notification,
    IUser appUser,
    IApplicationResponse applicationResponse,
    IMapper mapper,
    IProductAppService productAppService) : MainController(notifier, notification, appUser, applicationResponse, mapper)
{
    #region Routes

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Add(ProductDTO productDTO)
    {
        var product = await productAppService.AddAsync(productDTO, true);

        return CustomResponse(product);
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAll()
    {
        var products = await productAppService.GetAllAsync(true);

        return CustomResponse(products);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await productAppService.GetByIdAsync(id, true);

        return CustomResponse(product);
    }

    [HttpGet("get-products-suppliers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetProductsSuppliers()
    {
        var products = await productAppService.GetProductsSuppliersAsync();

        return CustomResponse(products);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(Guid id, ProductDTO productDTO)
    {
        var product = await productAppService.UpdateAsync(id, productDTO);

        return CustomResponse(product);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Remove(Guid id)
    {
        var product = await productAppService.RemoveAsync(id, true);

        return CustomResponse(product);
    }

    #endregion
}
