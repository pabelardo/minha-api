using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Extensions;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;

namespace MyApiV8.V1.Controllers.Base;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected readonly INotifier _notifier;
    protected readonly INotification _notification;
    protected readonly IMapper _mapper;
    public readonly IUser _appUser;
    public IApplicationResponse _applicationResponse;
    protected Guid UsuarioId { get; set; }
    protected bool UsuarioAutenticado { get; set; }

    protected MainController(
        INotifier notificador,
        INotification notification,
        IUser appUser,
        IApplicationResponse applicationResponse,
        IMapper mapper)
    {
        _notifier = notificador;
        _notification = notification;
        _applicationResponse = applicationResponse;
        _appUser = appUser;
        _mapper = mapper;

        if (!_appUser.IsAuthenticated()) return;

        UsuarioId = _appUser.GetUserId();
        UsuarioAutenticado = true;
    }

    protected bool ValidOperation() => !_notifier.HasNotification();

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notify(error.ErrorMessage);
    }

    protected void Notify(string message) => _notifier.Handle(_notification.CreateNotification(message));

    protected IActionResult CustomResponse(IApplicationResponse response)
    {
        if (ValidOperation()) return Ok(response);

        if (!response.Errors.Any() && _notifier.HasNotification())
            response.Errors = _notifier.GetNotifications().Select(n => n.Message);

        return response.StatusCode switch
        {
            (int)StatusCodeEnum.BadRequest => BadRequest(response),
            (int)StatusCodeEnum.NotFound => NotFound(response),
            (int)StatusCodeEnum.Unauthorized => Unauthorized(response),
            (int)StatusCodeEnum.InternalServerError => Problem(
                response.InnerException,
                HttpContext.Request.HttpContext.Request.Path,
                (int)StatusCodeEnum.InternalServerError,
                response.Errors.JoinToString()),
            _ => BadRequest(response)
        };
    }

    protected async Task<bool> Validate<T>(T obj) where T : class
    {
        try
        {
            Type genericType = typeof(IValidator<>).MakeGenericType(typeof(T));

            if (ConfigurationHelper.GetService(genericType) is not IValidator<T> validator)
            {
                Notify("Validator not found");
                return false;
            }

            var validation = await validator.GetValidationResult(obj);

            if (validation.IsValid) return true;

            Notify(validation);

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected async Task<bool> Validate<TV, TM>(TV validation, TM model)
        where TV : AbstractValidator<TM>
        where TM : class
    {
        var validationResult = await validation.ValidateAsync(model);

        if (validationResult.IsValid) return true;

        Notify(validationResult);

        return false;
    }
}