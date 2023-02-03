using System;

namespace ItemsAdministration.Common.Shared.Exceptions;

[Serializable]
public abstract class BaseLocalizedException : Exception
{
    protected BaseLocalizedException(string code, object? messageParameter = null)
    {
        Code = code;
        MessageParameter = messageParameter;
    }

    public string Code { get; }
    public object? MessageParameter { get; }
}
