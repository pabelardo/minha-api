namespace MyApiV8.Extensions;

public class JwtSettings
{
    public string Secret { get; set; }
    public int ExpiresIn { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}