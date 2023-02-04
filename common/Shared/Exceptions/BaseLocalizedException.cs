using System;

namespace ItemsAdministration.Common.Shared.Exceptions;

[Serializable]
public abstract class BaseLocalizedException : Exception
{
    protected BaseLocalizedException(string prefixCode, object? messageParameter = null)
    {
        PrefixCode = prefixCode;
        MessageParameter = messageParameter;
    }

    public string PrefixCode { get; }
    public object? MessageParameter { get; }
}
