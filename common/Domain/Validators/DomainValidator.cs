using ItemsAdministration.Common.Domain.Exception;
using ItemsAdministration.Common.Shared.Errors;
using ItemsAdministration.Common.Shared.Validators;

namespace ItemsAdministration.Common.Domain.Validators;

public abstract class DomainValidator<T> : BaseValidator<T>
{
    public void ValidateAndThrow(T candidate)
    {
        if (!IsValid(candidate, out LocalizedError[] errors))
        {
            throw new DomainValidationException(errors);
        };
    }
}