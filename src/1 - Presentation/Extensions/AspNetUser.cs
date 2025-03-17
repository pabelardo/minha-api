using MyApiV8.Domain.Interfaces.Repositories;
using System.Security.Claims;

namespace MyApiV8.Extensions;

public class AspNetUser(IHttpContextAccessor accessor) : IUser
{
    public string Name => accessor.HttpContext.User.Identity.Name;

    public Guid GetUserId() => IsAuthenticated() ? Guid.Parse(accessor.HttpContext.User.GetUserId()) : Guid.Empty; //Caso o guid seja vazio, validar aonde for chamado

    public string GetUserEmail() => IsAuthenticated() ? accessor.HttpContext.User.GetUserEmail() : "";

    public bool IsAuthenticated() => accessor.HttpContext.User.Identity.IsAuthenticated;

    public bool IsInRole(string role) => accessor.HttpContext.User.IsInRole(role);

    public IEnumerable<Claim> GetClaimsIdentity() => accessor.HttpContext.User.Claims;

    public string GetToken() => accessor.HttpContext.Request.Headers.Authorization;
}
