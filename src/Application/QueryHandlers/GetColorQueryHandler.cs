using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Interfaces.Repositories;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Application.Consts;
using ItemsAdministration.Application.Extensions;
using ItemsAdministration.Common.Application.Abstractions.Exceptions;
using ItemsAdministration.Common.Application.Abstractions.Interfaces.Queries;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.QueryHandlers;

internal sealed class GetColorQueryHandler : IQueryHandler<GetColorQuery, ColorResponse>
{
    private readonly IColorRepository _repository;

    public GetColorQueryHandler(IColorRepository repository) => 
        _repository = repository;

    public async Task<ColorResponse> Handle(GetColorQuery query, CancellationToken cancellationToken)
    {
        var color = await _repository.Get(query.Id);
        if (color is null)
            throw new ObjectNotFoundException(Names.Color, query);

        return color.ToResponse();
    }
}