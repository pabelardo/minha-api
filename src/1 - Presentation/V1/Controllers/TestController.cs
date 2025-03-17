using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.V1.Controllers.Base;

namespace MyApiV8.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/test")]
public class TestController(
    INotifier notificador,
    INotification notification,
    IUser appUser,
    IApplicationResponse applicationResponse,
    IMapper mapper) : MainController(notificador, notification, appUser, applicationResponse, mapper)
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("200OK");
    }
}
