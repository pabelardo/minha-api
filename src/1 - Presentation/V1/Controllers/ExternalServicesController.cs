using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApiV8.Application.Interfaces.Services;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.V1.Controllers.Base;

namespace MyApiV8.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/external")]
public class ExternalServicesController(
    IViaCepAppService _viaCepService,
    INotifier notificador,
    INotification notification,
    IUser appUser,
    IApplicationResponse applicationResponse,
    IMapper mapper) : MainController(notificador, notification, appUser, applicationResponse, mapper)
{
    #region Routes

    [HttpGet("get-address/{zipCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAddressByCep(string zipCode)
    {
        var address = await _viaCepService.GetAddresshByZipCode(zipCode);

        return CustomResponse(address);
    }

    #endregion
}
