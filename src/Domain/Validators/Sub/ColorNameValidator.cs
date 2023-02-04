using FluentValidation;
using ItemsAdministration.Common.Domain.Validators;
using ItemsAdministration.Domain.Consts;

namespace ItemsAdministration.Domain.Validators.Sub;

internal class ColorNameValidator : DomainValidator<string>
{
    private const int NameMinLength = 1;
    private const int NameMaxLength = 100;

    public ColorNameValidator()
    {
        RuleFor(colorName => colorName)
            .Length(NameMinLength, NameMaxLength)
            .WithErrorCode(ColorValidationCodes.ColorNameMinMaxRange)
            .WithState(_ => new { NameMinLength, NameMaxLength });
    }
}