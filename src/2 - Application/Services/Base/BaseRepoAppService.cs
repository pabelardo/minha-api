using AutoMapper;
using LanguageExt;
using MyApiV8.Application.Interfaces.Base;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Enums;
using MyApiV8.Domain.Interfaces.Models;
using MyApiV8.Domain.Interfaces.Notifier;
using MyApiV8.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace MyApiV8.Application.Services.Base;

public abstract class BaseRepoAppService<TEntity, TDTO>(
    INotifier notifier,
    INotification notification,
    IRepository<TEntity> repository,
    IMapper mapper,
    IApplicationResponse applicationResponse) : IBaseRepoAppService<TEntity, TDTO>
    where TEntity : Entity
    where TDTO : class
{
    protected readonly INotifier _notifier = notifier;
    protected readonly INotification _notification = notification;
    protected readonly IMapper _mapper = mapper;
    protected readonly IRepository<TEntity> _repository = repository;
    protected readonly IApplicationResponse _applicationResponse = applicationResponse;

    protected void Notify(string message) => _notifier.Handle(_notification.CreateNotification(message));

    public virtual async Task<TDTO> AddAsync(TDTO dto) =>
        _mapper.Map<TDTO>(await _repository.AddAsync(_mapper.Map<TEntity>(dto), true));

    public virtual async Task<IEnumerable<TDTO>> GetAllAsync() =>
        _mapper.Map<IEnumerable<TDTO>>(await _repository.GetAllAsync());

    public virtual async Task<TDTO> GetByIdAsync(Guid id) =>
        _mapper.Map<TDTO>(await _repository.GetByIdAsync(id));

    public virtual async Task<TDTO> UpdateAsync(TDTO dto) =>
        _mapper.Map<TDTO>(await _repository.UpdateAsync(_mapper.Map<TEntity>(dto)));

    public virtual Task RemoveAsync(Guid id) => _repository.RemoveAsync(id);

    public virtual Task<int> CountAsync() => _repository.CountAsync();

    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) =>
        _repository.CountAsync(predicate);

    public virtual Task<bool> ExistsAsync(Guid id) => _repository.ExistsAsync(id);

    public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
        _repository.ExistsAsync(predicate);

    public virtual async Task<IEnumerable<TDTO>> GetAllPaginatedAsync(int page, int pageSize) =>
        _mapper.Map<IEnumerable<TDTO>>(await _repository.GetAllPaginatedAsync(page, pageSize));

    public virtual async Task<IEnumerable<TDTO>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate) =>
        _mapper.Map<IEnumerable<TDTO>>(await _repository.GetAllPaginatedAsync(page, pageSize, predicate));

    public virtual async Task<IEnumerable<TDTO>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _mapper.Map<IEnumerable<TDTO>>(await _repository.GetByExpressionAsync(predicate));

    public virtual async Task<IEnumerable<TDTO>> GetByExpressionPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize) =>
        _mapper.Map<IEnumerable<TDTO>>(await _repository.GetByExpressionPaginatedAsync(predicate, page, pageSize));

    public virtual async Task<TDTO> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _mapper.Map<TDTO>(await _repository.GetFirstByExpressionAsync(predicate));

    public virtual async Task<TDTO> GetLastByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _mapper.Map<TDTO>(await _repository.GetLastByExpressionAsync(predicate));

    public virtual async Task<TDTO> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        _mapper.Map<TDTO>(await _repository.GetSingleByExpressionAsync(predicate));

    public virtual async Task<TDTO> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy) =>
        _mapper.Map<TDTO>(await _repository.GetSingleByExpressionAsync(predicate, orderBy));

    public virtual async Task<IApplicationResponse> AddAsync(TDTO dto, bool returnApplicationResponse = true) =>
        await _applicationResponse.Ok(_mapper.Map<TDTO>(await _repository.AddAsync(_mapper.Map<TEntity>(dto), true)));

    public virtual async Task<IApplicationResponse> GetAllAsync(bool withApplicationResponse = true) =>
        await _applicationResponse.Ok(_mapper.Map<IEnumerable<TDTO>>(await _repository.GetAllAsync()));

    public virtual async Task<IApplicationResponse> GetByIdAsync(Guid id, bool returnApplicationResponse = true)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
            return await _applicationResponse.NotFound();

        return await _applicationResponse.Ok(_mapper.Map<TDTO>(entity));
    }

    public virtual async Task<IApplicationResponse> GetAllPaginatedAsync(int page, int pageSize, bool returnApplicationResponse = true) =>
        await _applicationResponse.Ok(_mapper.Map<IEnumerable<TDTO>>(await _repository.GetAllPaginatedAsync(page, pageSize)));

    public virtual async Task<IApplicationResponse> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate, bool returnApplicationResponse = true) =>
        await _applicationResponse.Ok(_mapper.Map<IEnumerable<TDTO>>(await _repository.GetAllPaginatedAsync(page, pageSize, predicate)));

    public virtual async Task<IApplicationResponse> UpdateAsync(TDTO dto, bool returnApplicationResponse = true) =>
        await _applicationResponse.Ok(_mapper.Map<TDTO>(await _repository.UpdateAsync(_mapper.Map<TEntity>(dto))));

    public virtual async Task<IApplicationResponse> UpdateAsync(Guid id, TDTO dto, bool returnApplicationResponse = true) =>
        await _applicationResponse.Ok(_mapper.Map<TDTO>(await _repository.UpdateAsync(id, _mapper.Map<TEntity>(dto))));

    public virtual async Task<IApplicationResponse> RemoveAsync(Guid id, bool returnApplicationResponse = true)
    {
        if(!await _repository.ExistsAsync(id))
            return await _applicationResponse.NotFound();

        await _repository.RemoveAsync(id);
        return await _applicationResponse.Ok();
    }

    protected virtual async Task<IApplicationResponse> ApplicationResponse(StatusCodeEnum notOkStatus, string message = null)
    {
        message ??= string.Empty;

        if (!_notifier.HasNotification())
            return await _applicationResponse.Ok(message: message);

        return notOkStatus switch 
        { 
            StatusCodeEnum.NotFound => await _applicationResponse.NotFound(), 
            _ => await _applicationResponse.BadRequest() 
        };
    }

    protected virtual async Task<IApplicationResponse> ApplicationResponse(Option<TEntity> entity, StatusCodeEnum notOkStatus, string message = null)
    {
        message ??= string.Empty;

        return await entity.Match(async entity =>
            await _applicationResponse.Ok(_mapper.Map<TDTO>(entity), message),
            notOkStatus switch
            {
                StatusCodeEnum.NotFound => _applicationResponse.NotFound(_notifier.GetMessages()),
                _ => _applicationResponse.BadRequest()
            });
    }
}
