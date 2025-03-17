using LanguageExt;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Services.External;
using MyApiV8.Domain.Models;
using MyApiV8.Domain.Services.Base;

namespace MyApiV8.Infra.CrossCutting.Services.External;

public class ViaCepService(
    IHttpClientService httpService,
    INotifier notifier,
    INotification notification) : BaseService(notifier, notification), IViaCepService
{
    public async Task<Option<ViaCep>> GetAddressByZipCodeAsync(string zipCode)
    {
        if (!ValidateZipCode(zipCode))
            return Option<ViaCep>.None;

        string url = ConfigurationHelper.GetValueSection("ViaCEP", "Url");

        var newUrl = string.Format(url, zipCode);

        return await httpService.SendAsync<ViaCep>(HttpMethod.Get, newUrl);
    }

    private bool ValidateZipCode(string zipCode)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
        {
            Notify("Invalid zip code.");
            return false;
        }

        if (zipCode.All(char.IsDigit) == false)
        {
            Notify("ZIP code must only contain numbers.");
            return false;
        }

        if (zipCode.Length != 8)
        {
            Notify("ZIP code must contain 8 characters.");
            return false;
        }

        if (zipCode.Contains(' '))
        {
            Notify("ZIP code must not contain spaces.");
            return false;
        }

        return true;
    }
}