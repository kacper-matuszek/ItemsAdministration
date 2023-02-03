using System.Collections.Generic;
using ItemsAdministration.Common.Shared.Exceptions;

namespace ItemsAdministration.Common.Domain.Exception;

public class DomainValidationException : BaseManyLocalizedException
{
    public DomainValidationException(LocalizedError error)
        : base(error) { }

    public DomainValidationException(IReadOnlyCollection<LocalizedError> errors)
        : base(errors) { }
}