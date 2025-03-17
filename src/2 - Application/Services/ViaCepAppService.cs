using AutoMapper;
using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Services;
using MyApiV8.Application.Services.Base;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Services.External;
using MyApiV8.Domain.Models;

namespace MyApiV8.Application.Services;

public class ViaCepAppService(
    IViaCepService viaCepService,
    INotifier notifier,
    INotification notification,
    IMapper mapper,
    IApplicationResponse applicationResponse) : BaseAppService(notifier, notification, mapper, applicationResponse), IViaCepAppService
{
    public async Task<IApplicationResponse> GetAddresshByZipCode(string cep) => 
        await GetApplicationResponse<ViaCep, ViaCepDTO>(await viaCepService.GetAddressByZipCodeAsync(cep));
}