using Microsoft.AspNetCore.Identity;

namespace MyApiV8.Models;

public class User : IdentityUser<Guid>
{
    public List<IdentityRole<Guid>> Roles { get; set; }
}