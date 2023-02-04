using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Abstractions.Queries;

public record GetPaginatedColorsQuery : PaginatedQuery, IQuery<PaginatedList<ColorResponse>>
{
    public GetPaginatedColorsQuery(PaginatedQuery paginatedQuery)
    : base(paginatedQuery)
    { }
}
