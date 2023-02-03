using FluentValidation;
using ItemsAdministration.Common.Shared.Exceptions;
using System;
using System.Linq;

namespace ItemsAdministration.Common.Shared.Validators;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected const string PredicateValidatorCode = "PredicateValidator";

    protected bool IsValid(T candidate, out ValidationResultError[] exceptions)
    {
        exceptions = Array.Empty<ValidationResultError>();
        var validationResult = Validate(candidate);

        if (validationResult.IsValid)
        {
            return true;
        }

        exceptions = validationResult.Errors
            .Where(e => !e.ErrorCode.Equals(PredicateValidatorCode))
            .Select(e => new ValidationResultError(e.ErrorCode, e.CustomState))
            .ToArray();

        return false;
    }
}
