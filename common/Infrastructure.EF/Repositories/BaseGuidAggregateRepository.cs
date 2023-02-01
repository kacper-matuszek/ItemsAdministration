using System;
using ItemsAdministration.Common.Application.Interfaces.Repositories;
using ItemsAdministration.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsAdministration.Common.Infrastructure.EF.Repositories;

public abstract class BaseGuidAggregateRepository<TAggregate, TContext> 
    : BaseIdentifiableRepository<TAggregate, Guid, TContext>, 
        IBaseGuidAggregateRepository<TAggregate>
    where TAggregate : BaseGuidAggregate
    where TContext : DbContext
{
    protected BaseGuidAggregateRepository(TContext context)
        : base(context) { }
}