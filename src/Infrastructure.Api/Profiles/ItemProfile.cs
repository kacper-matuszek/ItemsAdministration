using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Common.Shared.Extensions;
using ItemsAdministration.Common.Shared.Requests;
using ItemsAdministration.Domain.Dtos;
using ItemsAdministration.PublishedLanguage.Requests;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Infrastructure.Api.Profiles;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<CreateItemRequest, CreateItemDto>();
        CreateMap<CreateItemRequest, CreateItemCommand>()
            .ConstructUsing((r, c) => new CreateItemCommand(c.Mapper.Map<CreateItemDto>(r)));

        CreateMap<UpdateItemRequest, UpdateItemDto>();
        CreateMap<UpdateItemRequest, UpdateItemCommand>()
            .ConstructUsing((r, c) => new UpdateItemCommand(r.Id, c.Mapper.Map<UpdateItemDto>(r)));

        this.ConstructUsingWithIgnoreAll<GetPaginatedItemsRequest, GetPaginatedItemsQuery>((r, ctx) =>
            new GetPaginatedItemsQuery(ctx.Mapper.Map<IPaginatedListFilterRequest, PaginatedQuery>(r)));

        this.AddPaginatedListMapping<ItemResponse, ItemResponse>();
    }
}