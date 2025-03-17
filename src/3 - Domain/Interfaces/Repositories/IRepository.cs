using MyApiV8.Domain.Entities;
using System.Linq.Expressions;

namespace MyApiV8.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task AddAsync(TEntity entity);
    Task<TEntity> AddAsync(TEntity entity, bool returnEntity = true);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(Guid id, TEntity entity);
    Task RemoveAsync(Guid id);
    Task<int> SaveChangesAsync();
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize);
    Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetByExpressionPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);
    Task<TEntity?> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetLastByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
}