using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ItemsAdministration.Common.Domain.Models.Interfaces;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Repositories;

public interface IBaseIdentifiableRepository<TEntity, TId>
    where TEntity : class, IIdentifiable<TId>
    where TId : notnull
{
    Task<TEntity?> Get(TId id);
    Task<TEntity?> Get(Expression<Func<TEntity, bool>> condition);
    Task<IReadOnlyList<TEntity>> GetMany(IEnumerable<TId> ids);
    Task<IReadOnlyList<TEntity>> GetMany(Expression<Func<TEntity, bool>> condition);
    Task Add(TEntity entity);
    Task AddMany(IReadOnlyCollection<TEntity> entities);
    Task Update(TEntity entity);
    Task UpdateMany(IReadOnlyCollection<TEntity> entities);
    Task<bool> Any(TId id);
    Task<bool> Any(Expression<Func<TEntity, bool>> condition);
    Task Delete(TEntity entity);
    Task DeleteMany(IReadOnlyCollection<TEntity> entities);
}