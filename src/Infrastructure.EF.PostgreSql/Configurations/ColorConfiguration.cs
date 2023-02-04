using ItemsAdministration.Common.Infrastructure.EF.Configurations;
using ItemsAdministration.Domain.Models;
using ItemsAdministration.Infrastructure.EF.PostgreSql.Seeds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemsAdministration.Infrastructure.EF.PostgreSql.Configurations;

internal sealed class ColorConfiguration : BaseGuidAggregateConfiguration<Color>
{
    private const int NameMaxLength = 100;

    public override void Configure(EntityTypeBuilder<Color> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Name)
               .HasMaxLength(NameMaxLength);

        builder.HasIndex(e => e.Name)
               .IsUnique();

        builder.HasData(ColorSeedData.Seed());
    }
}