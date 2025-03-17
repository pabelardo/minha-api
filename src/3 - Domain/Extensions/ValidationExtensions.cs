using FluentValidation;
using FluentValidation.Results;

namespace MyApiV8.Domain.Extensions;

public static class ValidationExtensions
{
    public static bool IsCpf(this string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return false;

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");

        if (cpf.Length != 11) return false;

        if (cpf.Distinct().Count() == 1) return false;

        if (cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999") return false;

        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        int rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;

        string digit = rest.ToString();
        tempCpf += digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;

        digit += rest.ToString();

        return cpf.EndsWith(digit);
    }

    public static bool IsCnpj(this string cnpj)
    {
        if (string.IsNullOrEmpty(cnpj)) return false;

        cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14) return false;

        if (cnpj.Distinct().Count() == 1) return false;

        int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

        int rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;

        string digit = rest.ToString();
        tempCnpj += digit;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

        rest = sum % 11;
        rest = rest < 2 ? 0 : 11 - rest;

        digit += rest.ToString();

        return cnpj.EndsWith(digit);
    }

    public static async Task<ValidationResult> GetValidationResult<T>(this IValidator<T> validator, T instance) =>
        await validator.ValidateAsync(instance);
}