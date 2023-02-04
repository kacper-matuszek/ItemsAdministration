using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Abstractions.Interfaces.ReadModels;

public interface IPaginatedItemsReadModel
{
    Task<PaginatedList<ItemResponse>> Get(GetPaginatedItemsQuery query, CancellationToken cancellationToken);
}