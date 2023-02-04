using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.Common.Shared.Responses;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Abstractions.Queries;

public record GetPaginatedItemsQuery : PaginatedQuery, IQuery<PaginatedList<ItemResponse>>
{
    public GetPaginatedItemsQuery(PaginatedQuery paginatedQuery)
        : base(paginatedQuery)
    { }
}