using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.V1.Controllers.Base;

namespace MyApiV8.V1.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/health-check")]
public class HealthCheckController(
    HealthCheckService healthCheckService,
    INotifier notificador,
    INotification notification,
    IUser appUser,
    IApplicationResponse applicationResponse,
    IMapper mapper) : MainController(notificador, notification, appUser, applicationResponse, mapper)
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var healthReport = await healthCheckService.CheckHealthAsync();

        return Ok(healthReport.Status.ToString());
    }
}
