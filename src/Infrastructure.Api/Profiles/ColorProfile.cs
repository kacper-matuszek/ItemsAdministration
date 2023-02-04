using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.PublishedLanguage.Requests;

namespace ItemsAdministration.Infrastructure.Api.Profiles;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<CreateColorRequest, CreateColorCommand>()
            .ConstructUsing(r => new CreateColorCommand(r.Name));

        CreateMap<UpdateColorRequest, UpdateColorCommand>()
            .ConstructUsing(r => new UpdateColorCommand(r.Id, r.Name));
    }
}