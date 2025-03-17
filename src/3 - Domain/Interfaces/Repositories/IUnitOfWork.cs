using MyApiV8.Domain.Entities;

namespace MyApiV8.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
    Task BeginTransactionAsync();
    Task<int> SaveChangesAsync();
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
}
