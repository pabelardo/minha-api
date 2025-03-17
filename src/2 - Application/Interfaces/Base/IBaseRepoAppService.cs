using MyApiV8.Domain.Interfaces.Models;
using System.Linq.Expressions;

namespace MyApiV8.Application.Interfaces.Base;

public interface IBaseRepoAppService<TEntity, TDTO>
    where TEntity : class
    where TDTO : class
{
    Task<TDTO> AddAsync(TDTO dto);
    Task<TDTO> UpdateAsync(TDTO dto);
    Task RemoveAsync(Guid id);
    Task<TDTO> GetByIdAsync(Guid id);
    Task<IEnumerable<TDTO>> GetAllAsync();
    Task<IEnumerable<TDTO>> GetAllPaginatedAsync(int page, int pageSize);
    Task<IEnumerable<TDTO>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TDTO>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TDTO>> GetByExpressionPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);
    Task<TDTO> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TDTO> GetLastByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TDTO> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TDTO> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

    Task<IApplicationResponse> AddAsync(TDTO dto, bool returnApplicationResponse = true);
    Task<IApplicationResponse> GetByIdAsync(Guid id, bool returnApplicationResponse = true);
    Task<IApplicationResponse> GetAllAsync(bool returnApplicationResponse = true);
    Task<IApplicationResponse> GetAllPaginatedAsync(int page, int pageSize, bool returnApplicationResponse = true);
    Task<IApplicationResponse> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate, bool returnApplicationResponse = true);
    Task<IApplicationResponse> UpdateAsync(TDTO dto, bool returnApplicationResponse = true);
    Task<IApplicationResponse> UpdateAsync(Guid id, TDTO dto, bool returnApplicationResponse = true);
    Task<IApplicationResponse> RemoveAsync(Guid id, bool returnApplicationResponse = true);
}
