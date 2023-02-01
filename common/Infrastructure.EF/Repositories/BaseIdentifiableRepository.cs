using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ItemsAdministration.Common.Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItemsAdministration.Common.Infrastructure.EF.Repositories;

public abstract class BaseIdentifiableRepository<TEntity, TId, TContext>
    where TEntity : class, IIdentifiable<TId>
    where TId : notnull
    where TContext : DbContext
{
    protected BaseIdentifiableRepository(TContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
        DbSetQuery = DbSet;
    }

    protected TContext Context { get; }
    protected DbSet<TEntity> DbSet { get; }
    protected IQueryable<TEntity> DbSetQuery { get; }

    public virtual async Task<TEntity?> Get(TId id) =>
        await Get(e => e.Id.Equals(id)).ConfigureAwait(false);

    public virtual async Task<TEntity?> Get(Expression<Func<TEntity, bool>> condition) => 
        GetFromLocal(condition.Compile()) ?? await DbSetQuery.FirstOrDefaultAsync(condition).ConfigureAwait(false);

    public virtual async Task<IReadOnlyList<TEntity>> GetMany(IEnumerable<TId> ids)
    {
        var uniqueIds = ids.ToHashSet();
        return await GetMany(p => uniqueIds.Contains(p.Id)).ConfigureAwait(false);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetMany(Expression<Func<TEntity, bool>> condition)
    {
        var entitiesFromLocal = GetManyFromLocal(condition.Compile());
        var entityIdsFromLocal = entitiesFromLocal.Select(e => e.Id).ToHashSet();

        Expression<Func<TEntity, bool>> exceptLocal = e => !entityIdsFromLocal.Contains(e.Id);
        var conditionParam = condition.Parameters.First();
        var entitiesWithoutLocalExpression = Expression.AndAlso(
            condition.Body, Expression.Invoke(exceptLocal, conditionParam));
        var entitiesWithoutLocalLambda =
            Expression.Lambda<Func<TEntity, bool>>(entitiesWithoutLocalExpression, conditionParam);

        var entitiesFromDb = await DbSetQuery.Where(entitiesWithoutLocalLambda).ToListAsync().ConfigureAwait(false);
        var entities = entitiesFromLocal.Concat(entitiesFromDb).ToList();

        return entities;
    }

    public async Task<bool> Any(TId id) =>
        await Any(e => e.Id.Equals(id)).ConfigureAwait(false);

    public async Task<bool> Any(Expression<Func<TEntity, bool>> condition) =>
        await DbSet.AnyAsync(condition).ConfigureAwait(false);

    public virtual async Task Add(TEntity entity)
    {
        await DbSet.AddAsync(entity).ConfigureAwait(false);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task AddMany(IReadOnlyCollection<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities).ConfigureAwait(false);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task Update(TEntity entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task UpdateMany(IReadOnlyCollection<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task DeleteMany(IReadOnlyCollection<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    private TEntity? GetFromLocal(Func<TEntity, bool> condition) =>
        DbSet.Local.FirstOrDefault(condition);

    private IReadOnlyList<TEntity> GetManyFromLocal(Func<TEntity, bool> condition) =>
        DbSet.Local.Where(condition).ToList();
}