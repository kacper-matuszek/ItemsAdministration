using ItemsAdministration.Common.Domain.Validators;
using ItemsAdministration.Domain.Models;
using ItemsAdministration.Domain.Validators.Sub;

namespace ItemsAdministration.Domain.Validators;

public class ColorValidator : DomainValidator<Color>
{
    public ColorValidator()
    {
        RuleFor(e => e.Name)
            .SetValidator(new ColorNameValidator());
    }
}