using Microsoft.EntityFrameworkCore;
using MyApiV8.Domain.Entities;
using MyApiV8.Domain.Interfaces.Repositories;
using MyApiV8.Infra.Data.Context;
using System.Linq.Expressions;

namespace MyApiV8.Infra.Data.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly MyDbContext Db;
    protected readonly DbSet<TEntity> DbSet;
    protected Repository(MyDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate) =>
        await DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await DbSet.FindAsync(id);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await DbSet.ToListAsync();

    public virtual async Task AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, bool returnEntity = true)
    {
        try
        {
            var result = await DbSet.AddAsync(entity);
            await SaveChangesAsync();
            return returnEntity ? result.Entity : null!;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var result = DbSet.Update(entity);
        await SaveChangesAsync();
        return result.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        if (id != entity.Id)
            throw new InvalidOperationException("The id is not the same as the object to be updated.");

        var result = DbSet.Update(entity);
        await SaveChangesAsync();
        return result.Entity;
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });
        await SaveChangesAsync();
    }

    public Task<int> SaveChangesAsync() =>
        Db.SaveChangesAsync();

    public Task<int> CountAsync() => DbSet.CountAsync();

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) => 
        DbSet.CountAsync(predicate);

    public Task<bool> ExistsAsync(Guid id) => DbSet.AnyAsync(e => e.Id == id);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
        DbSet.AnyAsync(predicate);

    public async Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = DbSet.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        return await DbSet.Skip(skip).Take(pageSize).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
    {
        var skip = (page - 1) * pageSize;
        return await DbSet.Where(predicate).Skip(skip).Take(pageSize).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate) => 
        await DbSet.AsQueryable().Where(predicate).ToListAsync();

    public async Task<IEnumerable<TEntity>> GetByExpressionPaginatedAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        return await DbSet.Where(predicate).Skip(skip).Take(pageSize).ToListAsync();
    }

    public Task<TEntity?> GetFirstByExpressionAsync(Expression<Func<TEntity, bool>> predicate) => 
        DbSet.AsQueryable().FirstOrDefaultAsync(predicate);

    public Task<TEntity?> GetLastByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        DbSet.AsQueryable().LastOrDefaultAsync(predicate);

    public Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate) =>
        DbSet.AsQueryable().SingleAsync(predicate);

    public Task<TEntity> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy) => 
        DbSet.AsQueryable().SingleAsync(predicate);

    public void Dispose() => GC.SuppressFinalize(this);
}
