using Asp.Versioning;
using AutoMapper;
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyApiV8.Application.DTOs;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Domain.Models;
using MyApiV8.Domain.Utils;
using MyApiV8.Models;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MyApiV8.V1.Controllers.Base;

namespace MyApiV8.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController(
    INotifier notifier,
    INotification notification,
    IUser appUser,
    IApplicationResponse applicationResponse,
    IMapper mapper,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IOptions<JwtSettings> jwtSettings,
    ILogger<AuthController> logger,
    IJwtService jwtService) :
    MainController(notifier, notification, appUser, applicationResponse, mapper)
{
    #region Dependency Injection

    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly ILogger _logger = logger;

    #endregion

    #region Routes

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO registerUser)
    {
        if (!await Validate(registerUser))
            return CustomResponse(await _applicationResponse.BadRequest());

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

        var user = new User
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            PasswordHash = passwordHash,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, registerUser.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                _notifier.Handle(_notification.CreateNotification(error.Description));

            return CustomResponse(await _applicationResponse.BadRequest());
        }

        await signInManager.SignInAsync(user, false);

        var loginResponse = await GenerateLoginResponse(user.Email);

        return CustomResponse(await _applicationResponse.Ok(_mapper.Map<LoginResponseDTO>(loginResponse)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO loginUser)
    {
        if (!await Validate(loginUser))
            return CustomResponse(await _applicationResponse.BadRequest());

        var user = await userManager.FindByEmailAsync(loginUser.Email);

        var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

        if (user == null)
        {
            Notify("Invalid user.");
            return CustomResponse(await _applicationResponse.BadRequest());
        }

        if (!result.Succeeded)
        {
            Notify("Invalid user or password.");
            return CustomResponse(await _applicationResponse.BadRequest());
        }

        var loginResponse = await GenerateLoginResponse(user.Email);

        return CustomResponse(await _applicationResponse.Ok(_mapper.Map<LoginResponseDTO>(loginResponse)));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(TokenDTO tokenModel)
    {
        if (!await Validate(tokenModel))
            return CustomResponse(await _applicationResponse.BadRequest());

        var key = await jwtService.GetCurrentSigningCredentials();

        var handler = new JsonWebTokenHandler();

        var result = await handler.ValidateTokenAsync(tokenModel.RefreshToken, new TokenValidationParameters
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.RefreshTokenAudience,
            IssuerSigningKey = key.Key,
        });

        if (!result.IsValid)
        {
            Notify("Expired Token.");
            return CustomResponse(await _applicationResponse.BadRequest());
        }

        var user = await userManager.FindByEmailAsync(result.Claims[System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email].ToString());
        var claims = await userManager.GetClaimsAsync(user);

        if (!claims.Any(c => c.Type == "LastRefreshToken" && c.Value == result.Claims[System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti].ToString()))
        {
            Notify("Expired token.");
            return CustomResponse(await _applicationResponse.BadRequest());
        }

        var loginResponse = _mapper.Map<LoginResponseDTO>(await GenerateLoginResponse(user.Email));

        return CustomResponse(await _applicationResponse.Ok(loginResponse));
    }

    #endregion

    #region Utils

    private async Task<LoginResponse> GenerateLoginResponse(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        var userRoles = await userManager.GetRolesAsync(user);

        var identityClaims = new ClaimsIdentity();

        var claims = await userManager.GetClaimsAsync(user);

        identityClaims.AddClaims(claims);
        identityClaims.AddClaims(userRoles.Select(s => new Claim(ClaimTypes.Role, s)));
        identityClaims.AddClaim(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        identityClaims.AddClaim(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email));
        identityClaims.AddClaim(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        identityClaims.AddClaim(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Nbf, Helpers.ToUnixEpochDate(DateTime.UtcNow).ToString()));
        identityClaims.AddClaim(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, Helpers.ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = await jwtService.GetCurrentSigningCredentials(),
            Subject = identityClaims,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiresInHours),
            IssuedAt = DateTime.UtcNow,
            TokenType = "access_token"
        });
        var accessToken = handler.WriteToken(token);

        var response = new LoginResponse
        {
            IsSuccessful = true,
            AccessToken = accessToken,
            ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpiresInHours).TotalSeconds,
            RefreshToken = await GenerateRefreshToken(user.Email),
            RefreshTokenExpiresIn = TimeSpan.FromHours(_jwtSettings.RefreshTokenValidityInHours).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles ?? [],
                Claims = claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value })
            }
        };

        return response;
    }

    private async Task<string> GenerateRefreshToken(string email)
    {
        var jti = Guid.NewGuid().ToString();

        var user = await userManager.FindByEmailAsync(email);

        var claims = await userManager.GetClaimsAsync(user);

        claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email.ToString()));
        claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, jti));

        var identityClaims = new ClaimsIdentity();

        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.RefreshTokenAudience,
            SigningCredentials = await jwtService.GetCurrentSigningCredentials(),
            Subject = identityClaims,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddHours(_jwtSettings.RefreshTokenValidityInHours),
            TokenType = "refresh_token"
        });

        await UpdateLastGeneratedClaim(email, jti);

        var encodedRt = tokenHandler.WriteToken(token);

        return encodedRt;
    }

    private async Task UpdateLastGeneratedClaim(string email, string jti)
    {
        var user = await userManager.FindByEmailAsync(email);
        var claims = await userManager.GetClaimsAsync(user);
        var newLastRtClaim = new Claim("LastRefreshToken", jti);

        var claimLastRt = claims.FirstOrDefault(f => f.Type == "LastRefreshToken");

        if (claimLastRt != null)
            await userManager.ReplaceClaimAsync(user, claimLastRt, newLastRtClaim);
        else
            await userManager.AddClaimAsync(user, newLastRtClaim);
    }

    #endregion
}
