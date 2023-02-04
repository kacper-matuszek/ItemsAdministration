using ItemsAdministration.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemsAdministration.Common.Infrastructure.EF.Configurations;

public abstract class BaseGuidAggregateConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseGuidAggregate
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Version)
            .IsRequired()
            .IsConcurrencyToken();
    }
}