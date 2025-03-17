using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Base;
using MyApiV8.Domain.Entities;

namespace MyApiV8.Application.Interfaces.Services;

public interface IAddressAppService : IBaseRepoAppService<Address, AddressDTO> { }