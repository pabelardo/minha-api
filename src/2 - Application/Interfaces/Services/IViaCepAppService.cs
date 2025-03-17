using MyApiV8.Application.Interfaces.Base;
using MyApiV8.Domain.Interfaces.Models;

namespace MyApiV8.Application.Interfaces.Services;

public interface IViaCepAppService : IBaseAppService
{
    Task<IApplicationResponse> GetAddresshByZipCode(string cep);
}