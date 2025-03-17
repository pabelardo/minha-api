using FluentValidation;
using FluentValidation.Results;
using LanguageExt;
using MyApiV8.Domain.Configuration;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Extensions;
using MyApiV8.Domain.Interfaces.Base;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace MyApiV8.Domain.Services.Base;

public abstract class BaseRepoService<TEntity> : IBaseRepoService<TEntity> where TEntity : Entity
{
    private readonly INotifier _notifier;
    private readonly INotification _notification;
    protected readonly IRepository<TEntity> _repository;

    protected BaseRepoService(
        IRepository<TEntity> repository,
        INotifier notifier,
        INotification notification)
    {
        _repository = repository;
        _notifier = notifier;
        _notification = notification;
    }

    private void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notify(error.ErrorMessage);
    }

    protected void Notify(string message) => _notifier.Handle(_notification.CreateNotification(message));

    protected async Task<bool> Validate<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
    {
        var validationResult = await validation.ValidateAsync(entity);

        if (validationResult.IsValid) return true;

        Notify(validationResult);

        return false;
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

    public virtual Task AddAsync(TEntity entity) => _repository.AddAsync(entity);

    public virtual async Task<Option<TEntity>> AddAsync(TEntity entity, bool returnEntity = true)
    {
        try
        {
            if (!await Validate(entity)) return Option<TEntity>.None;

            return await _repository.AddAsync(entity, true);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _repository.GetByFilterAsync(predicate);

    public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);

    public virtual async Task<Option<TEntity>> GetByIdOptionAsync(Guid id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            Notify("The object was not found.");
            return Option<TEntity>.None;
        }

        return await _repository.GetByIdAsync(id);
    }

    public virtual Task<IEnumerable<TEntity>> GetAllAsync() => _repository.GetAllAsync();

    public virtual async Task RemoveAsync(Guid id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            Notify("The object cannot be deleted because it was not found.");
            return;
        }

        await _repository.RemoveAsync(id);
    }

    public virtual async Task<Option<TEntity>> UpdateAsync(TEntity entity)
    {
        if (!await Validate(entity)) return Option<TEntity>.None;

        return await _repository.UpdateAsync(entity);
    }

    public virtual async Task<Option<TEntity>> UpdateAsync(Guid id, TEntity entity)
    {
        if(id != entity.Id)
        {
            Notify("The id is not the same as the object to be updated.");
            return Option<TEntity>.None;
        }

        if (!await Validate(entity)) return Option<TEntity>.None;

        return await _repository.UpdateAsync(entity);
    }

    public virtual Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize) =>
        _repository.GetAllPaginatedAsync(page, pageSize);

    public virtual Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate) =>
        _repository.GetAllPaginatedAsync(page, pageSize, predicate);

    public virtual Task<int> CountAsync() => _repository.CountAsync();

    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) => _repository.CountAsync(predicate);

    public virtual Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(id);

    public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) => _repository.ExistsAsync(predicate);

    public virtual Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _repository.GetByExpressionAsync(predicate);

    public virtual Task<IEnumerable<TEntity>> GetByExpressionPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize) =>
        _repository.GetByExpressionPaginatedAsync(predicate, page, pageSize);

    public virtual Task<TEntity> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _repository.GetFirstByExpressionAsync(predicate);

    public virtual Task<TEntity> GetLastByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _repository.GetLastByExpressionAsync(predicate);

    public virtual Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _repository.GetSingleByExpressionAsync(predicate);

    public virtual Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy) =>
        _repository.GetSingleByExpressionAsync(predicate, orderBy);
}
