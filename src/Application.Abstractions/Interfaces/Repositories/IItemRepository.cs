using ItemsAdministration.Common.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Domain.Models;

namespace ItemsAdministration.Application.Abstractions.Interfaces.Repositories;

public interface IItemRepository : IBaseGuidAggregateRepository<Item> 
{
}