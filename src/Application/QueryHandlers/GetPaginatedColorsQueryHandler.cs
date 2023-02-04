using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Interfaces.ReadModels;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.QueryHandlers;

internal class GetPaginatedColorsQueryHandler : IQueryHandler<GetPaginatedColorsQuery, PaginatedList<ColorResponse>>
{
    private readonly IPaginatedColorsReadModel _readModel;

    public GetPaginatedColorsQueryHandler(IPaginatedColorsReadModel readModel) => 
        _readModel = readModel;

    public Task<PaginatedList<ColorResponse>> Handle(GetPaginatedColorsQuery query, CancellationToken cancellationToken) =>
        _readModel.Get(query, cancellationToken);
}