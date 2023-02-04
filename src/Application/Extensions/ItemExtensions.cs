using ItemsAdministration.Domain.Models;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Extensions;

internal static class ItemExtensions
{
    public static ItemResponse ToResponse(this Item item) =>
        new ItemResponse()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Annotations = item.Annotations
        };
}