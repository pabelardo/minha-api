using System.Security.Claims;

namespace MyApiV8.Domain.Interfaces.Repositories;

public interface IUser
{
    string Name { get; }
    Guid GetUserId();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsInRole(string role);
    IEnumerable<Claim> GetClaimsIdentity();
    string GetToken();
}