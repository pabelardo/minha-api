using MyApiV8.Domain.Utils;

namespace MyApiV8.Domain.Validations.DocumentosValidation;

public static class CpfValidation
{
    public const int MaxLength = 11;

    public static bool Validar(string cpf)
    {
        var cpfNumeros = Helpers.OnlyNumbers(cpf);

        if (!TamanhoValido(cpfNumeros)) return false;
        return !TemDigitosRepetidos(cpfNumeros) && TemDigitosValidos(cpfNumeros);
    }

    private static bool TamanhoValido(string valor)
    {
        return valor.Length == MaxLength;
    }

    private static bool TemDigitosRepetidos(string valor)
    {
        string[] invalidNumbers =
        {
            "00000000000",
            "11111111111",
            "22222222222",
            "33333333333",
            "44444444444",
            "55555555555",
            "66666666666",
            "77777777777",
            "88888888888",
            "99999999999"
        };
        return invalidNumbers.Contains(valor);
    }

    private static bool TemDigitosValidos(string valor)
    {
        var number = valor[..(MaxLength - 2)];
        var digitoVerificador = new DigitoVerificador(number)
            .ComMultiplicadoresDeAte(2, 11)
            .Substituindo("0", 10, 11);
        var firstDigit = digitoVerificador.CalculaDigito();
        digitoVerificador.AddDigito(firstDigit);
        var secondDigit = digitoVerificador.CalculaDigito();

        return string.Concat(firstDigit, secondDigit) == valor.Substring(MaxLength - 2, 2);
    }
}