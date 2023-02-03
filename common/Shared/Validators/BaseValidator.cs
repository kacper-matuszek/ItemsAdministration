using FluentValidation;
using ItemsAdministration.Common.Shared.Exceptions;
using System;
using System.Linq;

namespace ItemsAdministration.Common.Shared.Validators;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected const string PredicateValidatorCode = "PredicateValidator";

    protected bool IsValid(T candidate, out ValidationResultException[] exceptions)
    {
        exceptions = Array.Empty<ValidationResultException>();
        var validationResult = Validate(candidate);

        if (validationResult.IsValid)
        {
            return true;
        }

        exceptions = validationResult.Errors
            .Where(e => !e.ErrorCode.Equals(PredicateValidatorCode))
            .Select(e => new ValidationResultException(e.ErrorCode, e.CustomState))
            .ToArray();

        return false;
    }
}
