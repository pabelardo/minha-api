using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Domain.Interfaces.Services;
using MyApiV8.Domain.Services.Base;

namespace MyApiV8.Domain.Services;

public class AddressService(
    INotifier notifier,
    INotification notification,
    IAddressRepository addressRepository) : BaseRepoService<Address>(addressRepository, notifier, notification), IAddressService
{
}