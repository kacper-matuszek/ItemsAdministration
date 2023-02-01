using System;

namespace ItemsAdministration.Common.Domain.Models;

public abstract class BaseGuidAggregate : BaseAggregate<Guid>
{
    protected BaseGuidAggregate(Guid id)
        : base(id) { }
}