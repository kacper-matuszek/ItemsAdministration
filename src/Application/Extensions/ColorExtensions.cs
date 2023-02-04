using ItemsAdministration.Domain.Models;
using ItemsAdministration.PublishedLanguage.Response;

namespace ItemsAdministration.Application.Extensions;

internal static class ColorExtensions
{
    public static ColorResponse ToResponse(this Color color) =>
        new ColorResponse { Id = color.Id, Name = color.Name };
}