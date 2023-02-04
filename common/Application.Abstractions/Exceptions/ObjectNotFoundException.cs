using System;
using ItemsAdministration.Common.Shared.Exceptions;

namespace ItemsAdministration.Common.Application.Abstractions.Exceptions;

public class ObjectNotFoundException : BaseLocalizedException
{
    private const string NotFoundCode = "NotFound";

    public ObjectNotFoundException(string prefixCode, object messageParameter)
        : base(CreateCode(prefixCode), messageParameter)
    {
    }

    private static string CreateCode(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentNullException(nameof(prefix));
        }

        return $"{prefix}{NotFoundCode}";
    }
}