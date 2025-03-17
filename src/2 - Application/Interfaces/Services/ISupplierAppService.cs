using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Base;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Models;

namespace MyApiV8.Application.Interfaces.Services;

public interface ISupplierAppService : IBaseRepoAppService<Supplier, SupplierDTO>
{
    Task<IApplicationResponse> GetSupplierAddressAsync(Guid id);
    Task<IApplicationResponse> GetSupplierProductsAddressAsync(Guid id);
}