using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Interfaces.ReadModels;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.QueryHandlers;

internal sealed class GetPaginatedItemsQueryHandler : IQueryHandler<GetPaginatedItemsQuery, PaginatedList<ItemResponse>>
{
    private readonly IPaginatedItemsReadModel _readModel;

    public GetPaginatedItemsQueryHandler(IPaginatedItemsReadModel readModel) =>
        _readModel = readModel;

    public Task<PaginatedList<ItemResponse>> Handle(GetPaginatedItemsQuery query, CancellationToken cancellationToken) =>
        _readModel.Get(query, cancellationToken);
}