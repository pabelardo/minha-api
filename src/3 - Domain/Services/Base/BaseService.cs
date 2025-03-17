using FluentValidation;
using FluentValidation.Results;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Extensions;
using MyApiV8.Domain.Interfaces.Base;
using MyApiV8.Domain.Interfaces.Notifier;

namespace MyApiV8.Domain.Services.Base;

public class BaseService : IBaseService
{
    protected readonly INotifier _notifier;
    protected readonly INotification _notification;

    public BaseService(
        INotifier notifier,
        INotification notification)
    {
        _notifier = notifier;
        _notification = notification;
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notify(error.ErrorMessage);
    }

    protected void Notify(string message) => _notifier.Handle(_notification.CreateNotification(message));

    public async Task<bool> Validate<T>(T obj) where T : class
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

    public async Task<bool> Validate<TV, TM>(TV validation, TM model)
        where TV : AbstractValidator<TM>
        where TM : class
    {
        var validationResult = await validation.ValidateAsync(model);

        if (validationResult.IsValid) return true;

        Notify(validationResult);

        return false;
    }
}