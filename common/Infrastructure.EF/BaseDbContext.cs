using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Common.Domain.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItemsAdministration.Common.Infrastructure.EF;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options)
        : base(options)
    { }

    public override int SaveChanges()
    {
        SaveEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    private void SaveEntities()
    {
        ChangeTracker.DetectChanges();
        var modifiedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .ToArray();

        foreach (var modifiedEntity in modifiedEntities)
        {
            (modifiedEntity as IPersistable)?.OnPersist();
        }
    }
}