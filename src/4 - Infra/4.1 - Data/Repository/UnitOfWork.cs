using Microsoft.EntityFrameworkCore.Storage;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Exceptions;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Infra.Data.Context;

namespace MyApiV8.Infra.Data.Repository;

public class UnitOfWork(MyDbContext dbContext) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];
    private IDbContextTransaction _transaction;
    private bool Disposed = false;

    public async Task BeginTransactionAsync() =>
        _transaction = await dbContext.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
    {
        if(_repositories.ContainsKey(typeof(TEntity)))
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];

        throw new RepositoryNotFoundException("Repositório não encontrado.");
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null!;
    }

    public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        if (disposing)
        {
            dbContext.Dispose();
        }

        Disposed = true;
    }
}
