namespace MyApiV8.Domain.Models;

public class LoginResponse
{
    public bool IsSuccessful { get; set; }
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public double RefreshTokenExpiresIn { get; set; }
    public UserToken UserToken { get; set; }
}