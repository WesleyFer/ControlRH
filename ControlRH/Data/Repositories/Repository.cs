﻿using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ControlRH.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entidade, IAggregateRoot
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    private static readonly EntityState[] s_estadosAtualizacao = new[]
    {
        EntityState.Detached,
        EntityState.Unchanged,
        EntityState.Modified
    };

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    //public async Task<IEnumerable<TEntity>> FindAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryBuilder = null, CancellationToken cancellationToken = default)
    //{
    //    IQueryable<TEntity> query = _dbContext.Set<TEntity>();

    //    if (queryBuilder != null)
    //    {
    //        query = queryBuilder(query);
    //    }

    //    return await query.ToListAsync(cancellationToken);
    //}

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = _dbSet.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        return query;
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}
