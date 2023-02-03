using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ItemsAdministration.Common.Shared.Exceptions;

[Serializable]
public abstract class BaseManyLocalizedException : Exception
{
    private readonly List<LocalizedError> _errors = new List<LocalizedError>();

    protected BaseManyLocalizedException(string code)
        : this(new LocalizedError(code, null))
    {
    }

    protected BaseManyLocalizedException(string code, object messageParameter)
        : this(new LocalizedError(code, messageParameter))
    {
    }

    protected BaseManyLocalizedException(LocalizedError error)
        : this(new List<LocalizedError> { error })
    {
    }

    protected BaseManyLocalizedException(IReadOnlyCollection<LocalizedError> errors)
    {
        _errors.AddRange(errors);
    }

    protected BaseManyLocalizedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public IReadOnlyList<LocalizedError> Errors => _errors;
}
