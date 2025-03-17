using LanguageExt;
using MyApiV8.Domain.Models;

namespace MyApiV8.Domain.Interfaces.Services.External;

public interface IViaCepService
{
    Task<Option<ViaCep>> GetAddressByZipCodeAsync(string zipCode);
}
