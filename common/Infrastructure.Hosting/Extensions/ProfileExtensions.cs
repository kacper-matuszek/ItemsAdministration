using System.Collections.Generic;
using AutoMapper;
using ItemsAdministration.Common.Application.Abstractions.Lists;
using ItemsAdministration.Common.Shared.Responses;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Extensions;

public static class ProfileExtensions
{
    public static void AddListMapping<TModel, TResponse>(this Profile profile)
    {
        profile.CreateMap<List<TModel>, ListResponse<TResponse>>()
            .ForMember(r => r.Elements, opt => opt.MapFrom(src => src));
    }

    public static void AddPaginatedListMapping<TModel, TResponse>(this Profile profile)
    {
        profile.CreateMap<PaginatedList<TModel>, PaginatedListResponse<TResponse>>()
            .ForMember(r => r.Elements, opt => opt.MapFrom(dto => dto.Elements))
            .ForMember(r => r.PagingResponse, opt => opt.MapFrom(dto => new PagingResponse
            {
                PagesCount = dto.PagesCount,
                TotalItemsCount = dto.TotalItemsCount
            }));
    }
}