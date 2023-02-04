using ItemsAdministration.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Common.Infrastructure.Attributes;
using ItemsAdministration.Common.Infrastructure.EF.Repositories;
using ItemsAdministration.Domain.Models;

namespace ItemsAdministration.Infrastructure.EF.PostgreSql.Repositories;

[Repository]
public class ItemRepository : BaseGuidAggregateRepository<Item, ItemsAdministrationDbContext>, IItemRepository
{
    public ItemRepository(ItemsAdministrationDbContext context)
        : base(context) { }
}