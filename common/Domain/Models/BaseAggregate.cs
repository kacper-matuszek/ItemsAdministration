using ItemsAdministration.Common.Domain.Models.Interfaces;

namespace ItemsAdministration.Common.Domain.Models;

public abstract class BaseAggregate<T> : IIdentifiable<T>, IPersistable, IVersionable
    where T : notnull
{
    private bool _versionIncremented;

    public T Id { get; }
    public long Version { get; private set; } = 1;

    protected BaseAggregate(T id) => 
        Id = id;

    public void OnPersist()
    {
        IncrementVersion();
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