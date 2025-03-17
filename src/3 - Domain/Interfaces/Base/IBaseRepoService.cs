using LanguageExt;
using MyApiV8.Domain.Entities;
using System.Linq.Expressions;

namespace MyApiV8.Domain.Interfaces.Base;

public interface IBaseRepoService<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity entity);
    Task<Option<TEntity>> AddAsync(TEntity entity, bool returnEntity = true);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<Option<TEntity>> GetByIdOptionAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize);
    Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetByExpressionPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);
    Task<TEntity> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetLastByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
    Task<Option<TEntity>> UpdateAsync(TEntity entity);
    Task<Option<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task RemoveAsync(Guid id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
