using MyApiV8.Domain.Utils;

namespace MyApiV8.Domain.Validations.DocumentosValidation;

public static class CnpjValidation
{
    public const int MaxLength = 14;

    public static bool Validar(string cpnj)
    {
        var cnpjNumeros = Helpers.OnlyNumbers(cpnj);
        if (!TemTamanhoValido(cnpjNumeros)) return false;
        return !TemDigitosRepetidos(cnpjNumeros) && TemDigitosValidos(cnpjNumeros);
    }

    private static bool TemTamanhoValido(string valor)
    {
        return valor.Length == MaxLength;
    }

    private static bool TemDigitosRepetidos(string valor)
    {
        string[] invalidNumbers =
        {
            "00000000000000",
            "11111111111111",
            "22222222222222",
            "33333333333333",
            "44444444444444",
            "55555555555555",
            "66666666666666",
            "77777777777777",
            "88888888888888",
            "99999999999999"
        };
        return invalidNumbers.Contains(valor);
    }

    private static bool TemDigitosValidos(string valor)
    {
        var number = valor[..(MaxLength - 2)];

        var digitoVerificador = new DigitoVerificador(number)
            .ComMultiplicadoresDeAte(2, 9)
            .Substituindo("0", 10, 11);
        var firstDigit = digitoVerificador.CalculaDigito();
        digitoVerificador.AddDigito(firstDigit);
        var secondDigit = digitoVerificador.CalculaDigito();

        return string.Concat(firstDigit, secondDigit) == valor.Substring(MaxLength - 2, 2);
    }
}