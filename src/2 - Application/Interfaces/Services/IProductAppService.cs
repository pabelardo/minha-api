using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Base;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Models;

namespace MyApiV8.Application.Interfaces.Services;

public interface IProductAppService : IBaseRepoAppService<Product, ProductDTO> 
{
    Task<IApplicationResponse> GetProductSupplierAsync(Guid id);
    Task<IApplicationResponse> GetProductsSuppliersAsync();
    Task<IApplicationResponse> GetProductsBySupplierIdAsync(Guid supplierId);
}