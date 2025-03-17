using Microsoft.AspNetCore.Identity;

namespace MyApiV8.Domain.Models;

public class UserToken
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<ClaimModel> Claims { get; set; }
    public List<IdentityRole<Guid>> Roles { get; set; }

    public UserToken()
    {
        Claims = [];
        Roles = [];
    }
}