using FluentValidation;
using ItemsAdministration.Common.Domain.Validators;
using ItemsAdministration.Domain.Consts;
using ItemsAdministration.Domain.Models;
using ItemsAdministration.Domain.Validators.Sub;

namespace ItemsAdministration.Domain.Validators;

public class ItemValidator : DomainValidator<Item>
{
    private const int CodeMinLength = 1;
    private const int CodeMaxLength = 12;

    private const int NameMinLength = 1;
    private const int NameMaxLength = 200;

    public ItemValidator()
    {
        RuleFor(e => e.Color)
            .Length(CodeMinLength, CodeMaxLength)
            .WithErrorCode(ItemValidationCodes.ItemCodeMinMaxRange)
            .WithState(_ => new { CodeMinLength, CodeMaxLength });

        RuleFor(e => e.Name)
            .Length(NameMinLength, NameMaxLength)
            .WithErrorCode(ItemValidationCodes.ItemNameMinMaxRange)
            .WithState(_ => new { NameMinLength, NameMaxLength });

        RuleFor(e => e.Color)
            .SetValidator(new ColorNameValidator());
    }
}