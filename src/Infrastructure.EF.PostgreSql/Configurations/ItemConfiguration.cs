using ItemsAdministration.Common.Infrastructure.EF.Configurations;
using ItemsAdministration.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemsAdministration.Infrastructure.EF.PostgreSql.Configurations;

internal sealed class ItemConfiguration : BaseGuidAggregateConfiguration<Item>
{
    private const int CodeMaxLength = 12;
    private const int NameMaxLength = 200;

    public override void Configure(EntityTypeBuilder<Item> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Code)
               .HasMaxLength(CodeMaxLength);

        builder.HasIndex(e => e.Code)
               .IsUnique();

        builder.Property(e => e.Name)
               .HasMaxLength(NameMaxLength);
    }
}