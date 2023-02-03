using ItemsAdministration.Common.Infrastructure.EF;
using ItemsAdministration.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsAdministration.Infrastructure.EF.PostgreSql;

public class ItemsAdministrationDbContext : BaseDbContext
{
    public ItemsAdministrationDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Item> Items { get; set; } = null!;
}