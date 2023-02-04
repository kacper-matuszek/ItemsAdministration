using System.Threading;
using System.Threading.Tasks;
using ItemsAdministration.Application.Abstractions.Interfaces.ReadModels;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.Common.Infrastructure.Options;
using ItemsAdministration.Common.Infrastructure.ReadModel;
using ItemsAdministration.Common.Infrastructure.ReadModel.Dapper.ReadModels;
using ItemsAdministration.Common.Infrastructure.ReadModel.Factories;
using ItemsAdministration.PublishedLanguage.Response;
using Microsoft.Extensions.Options;

namespace ItemsAdministration.Infrastructure.ReadModel;

[ReadModel]
public class PaginatedItemsReadModel : BaseSortableReadModel, IPaginatedItemsReadModel
{
    public PaginatedItemsReadModel(ISqlFactory connectionFactory, IOptions<PostgresDatabaseOptions> databaseOptions)
        : base(connectionFactory, databaseOptions) { }

    protected override string DefaultSortingColumnName => nameof(ItemResponse.Code);

    public Task<PaginatedList<ItemResponse>> Get(GetPaginatedItemsQuery query, CancellationToken cancellationToken)
    {
        const string sqlQuery = $@"
SELECT
    Id,
    Code,
    Name,
    Color,
    Annotations
FROM Items";

        return GetSimplePaginatedList<GetPaginatedItemsQuery, ItemResponse>(query, sqlQuery,null, cancellationToken);
    }
}