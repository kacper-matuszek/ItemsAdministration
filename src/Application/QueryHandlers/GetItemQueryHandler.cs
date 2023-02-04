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

internal sealed class GetItemQueryHandler : IQueryHandler<GetItemQuery, ItemResponse>
{
    private readonly IItemRepository _repository;

    public GetItemQueryHandler(IItemRepository repository) => 
        _repository = repository;

    public async Task<ItemResponse> Handle(GetItemQuery query, CancellationToken cancellationToken)
    {
        var item = await _repository.Get(query.Id);
        if (item is null)
            throw new ObjectNotFoundException(Names.Item, query);

        return item.ToResponse();
    }
}