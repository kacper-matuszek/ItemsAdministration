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
internal class PaginatedColorsReadModel : BaseSortableReadModel, IPaginatedColorsReadModel
{
    public PaginatedColorsReadModel(ISqlFactory connectionFactory, IOptions<PostgresDatabaseOptions> databaseOptions)
        : base(connectionFactory, databaseOptions) { }

    protected override string DefaultSortingColumnName => nameof(ColorResponse.Name);

    public Task<PaginatedList<ColorResponse>> Get(GetPaginatedColorsQuery query, CancellationToken cancellationToken)
    {
        const string sqlQuery = $@"
SELECT
    Id,
    Name
FROM Colors";

        return GetSimplePaginatedList<GetPaginatedColorsQuery, ColorResponse>(query, sqlQuery, null, cancellationToken);
    }
}