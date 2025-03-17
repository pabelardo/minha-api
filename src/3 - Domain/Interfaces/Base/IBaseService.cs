using FluentValidation;

namespace MyApiV8.Domain.Interfaces.Base;

public interface IBaseService
{
    Task<bool> Validate<T>(T obj) where T : class;
    Task<bool> Validate<TV, TM>(TV validation, TM entity) where TV : AbstractValidator<TM> where TM : class;
}
