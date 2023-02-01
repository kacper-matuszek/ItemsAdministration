using System;
using ItemsAdministration.Common.Domain.Models.Interfaces;

namespace ItemsAdministration.Common.Domain.Models;

public abstract class BaseAggregate<T> : IIdentifiable<T>, IPersistable, IVersionable, IAuditable
    where T : notnull
{
    private bool _versionIncremented;

    public T Id { get; }
    public long Version { get; private set; } = 1;

    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    protected BaseAggregate(T id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public void OnPersist()
    {
        IncrementVersion();
        UpdatedAt = DateTime.UtcNow;
    }

    private void IncrementVersion()
    {
        if (_versionIncremented)
        {
            return;
        }

        Version++;
        _versionIncremented = true;
    }
}