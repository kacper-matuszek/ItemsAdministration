using AutoMapper;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Common.Shared.Extensions;
using ItemsAdministration.Common.Shared.Requests;

namespace ItemsAdministration.Common.Infrastructure.Hosting;

public class QueryProfile : Profile
{
    public QueryProfile()
    {
        this.ConstructUsingWithIgnoreAll<ISortedListFilterRequest, SortingOptions>((src, _) =>
            src.GetSortingOptions());

        this.ConstructUsingWithIgnoreAll<ISortedListFilterRequest, SortedQuery>((vm, ctx) =>
            new SortedQuery(ctx.Mapper.Map<ISortedListFilterRequest, SortingOptions>(vm)));

        this.ConstructUsingWithIgnoreAll<IPaginatedListFilterRequest, PaginatedQuery>((vm, ctx) =>
            new PaginatedQuery(vm.PagingRequest.PageSize, vm.PagingRequest.PageNumber, vm.PagingRequest.GlobalSearch,
                ctx.Mapper.Map<ISortedListFilterRequest, SortingOptions>(vm)));
    }
}
