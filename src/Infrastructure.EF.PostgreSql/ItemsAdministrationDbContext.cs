using ItemsAdministration.Common.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace ItemsAdministration.Infrastructure.EF.PostgreSql;

public class ItemsAdministrationDbContext : BaseDbContext
{
    public ItemsAdministrationDbContext(DbContextOptions options)
        : base(options) { }
}