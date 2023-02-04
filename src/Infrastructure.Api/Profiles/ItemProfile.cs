using AutoMapper;
using ItemsAdministration.Application.Abstractions.Commands;
using ItemsAdministration.Domain.Dtos;
using ItemsAdministration.PublishedLanguage.Requests;

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
    }
}