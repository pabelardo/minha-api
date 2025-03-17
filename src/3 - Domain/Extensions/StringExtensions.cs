namespace MyApiV8.Domain.Extensions;

public static class StringExtensions
{
    public static string JoinToString(this IEnumerable<string> source, string delimiter = ",", char quote = '\'') =>
            string.Join(delimiter, source.Select(item => $"{quote}{item}{quote}"));
}