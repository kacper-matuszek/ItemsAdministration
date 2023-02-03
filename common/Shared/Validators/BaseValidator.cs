using FluentValidation;
using ItemsAdministration.Common.Shared.Exceptions;
using System;
using System.Linq;

namespace ItemsAdministration.Common.Shared.Validators;

public abstract class BaseValidator<T> : AbstractValidator<T>
{
    protected const string PredicateValidatorCode = "PredicateValidator";

    protected bool IsValid(T candidate, out LocalizedError[] exceptions)
    {
        exceptions = Array.Empty<LocalizedError>();
        var validationResult = Validate(candidate);

        if (validationResult.IsValid)
        {
            return true;
        }

        exceptions = validationResult.Errors
            .Where(e => !e.ErrorCode.Equals(PredicateValidatorCode))
            .Select(e => new LocalizedError(e.ErrorCode, e.CustomState))
            .ToArray();

        return false;
    }
}
