using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Application.Abstractions.Queries;
using ItemsAdministration.Common.Application.Abstractions.Queries;
using ItemsAdministration.Common.Infrastructure.Hosting.Extensions;
using ItemsAdministration.Common.Shared.Extensions;
using ItemsAdministration.Common.Shared.Requests;
using ItemsAdministration.PublishedLanguage.Requests;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Infrastructure.Api.Profiles;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<CreateColorRequest, CreateColorCommand>()
            .ConstructUsing(r => new CreateColorCommand(r.Name));

        CreateMap<UpdateColorRequest, UpdateColorCommand>()
            .ConstructUsing(r => new UpdateColorCommand(r.Id, r.Name));

        this.ConstructUsingWithIgnoreAll<GetPaginatedColorsRequest, GetPaginatedColorsQuery>((r, ctx) =>
            new GetPaginatedColorsQuery(ctx.Mapper.Map<IPaginatedListFilterRequest, PaginatedQuery>(r)));

        this.AddPaginatedListMapping<ColorResponse, ColorResponse>();
    }
}