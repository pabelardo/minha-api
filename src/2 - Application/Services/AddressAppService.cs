using AutoMapper;
using MyApiV8.Application.DTOs;
using MyApiV8.Application.Interfaces.Services;
using MyApiV8.Application.Services.Base;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;

namespace MyApiV8.Application.Services;

public class AddressAppService : BaseRepoAppService<Address, AddressDTO>, IAddressAppService
{
    private readonly IAddressRepository _addressRepository;

    public AddressAppService(
        INotifier notifier,
        INotification notification,
        IMapper mapper,
        IApplicationResponse applicationResponse,
        IAddressRepository addressRepository) : base(notifier, notification, addressRepository, mapper, applicationResponse)
    {
        _addressRepository = addressRepository;
    }
}