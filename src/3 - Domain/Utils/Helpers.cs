namespace MyApiV8.Domain.Utils;

public static class Helpers
{
    public static string OnlyNumbers(string valor)
    {
        var onlyNumber = valor.Where(char.IsDigit).Aggregate("", (current, s) => current + s);

        return onlyNumber.Trim();
    }

    public static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
