using System.Collections.Generic;
using ItemsAdministration.Domain.Models;

namespace ItemsAdministration.Infrastructure.EF.PostgreSql.Seeds;

internal static class ColorSeedData
{
    internal static IEnumerable<Color> Seed() =>
        new[]
        {
            new Color("Czarny"),
            new Color("Czerwony"),
            new Color("Zielony")
        };
}