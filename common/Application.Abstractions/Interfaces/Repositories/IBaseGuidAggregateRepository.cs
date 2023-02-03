using System;
using ItemsAdministration.Common.Domain.Models;

namespace ItemsAdministration.Common.Application.Abstractions.Interfaces.Repositories;

public interface IBaseGuidAggregateRepository<TAggregate> : IBaseIdentifiableRepository<TAggregate, Guid>
    where TAggregate : BaseGuidAggregate
{
}