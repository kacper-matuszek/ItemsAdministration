using ItemsAdministration.Common.Infrastructure.Hosting.Descriptions;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;

internal abstract class BaseLocalizedExceptionResponseFormatter<TException> : BaseExceptionResponseFormatter<TException>
    where TException : BaseLocalizedException
{
    protected BaseLocalizedExceptionResponseFormatter(
        ILogger<BaseLocalizedExceptionResponseFormatter<TException>> logger,
        ILocalizationService localizationService)
    : base(logger, localizationService)
    {
    }

    protected override IEnumerable<ErrorResponseDescription> GetErrorDescriptions(TException exception)
    {
        var message = LocalizationService.GetLocalizedString(exception.Code).Value;
        if (exception.MessageParameter is null)
        {
            yield return new ErrorResponseDescription(exception.Code, message);
        }

        var properties = exception.MessageParameter!
            .GetType()
            .GetProperties(System.Reflection.BindingFlags.Public | 
                           System.Reflection.BindingFlags.Instance);

        foreach(var property in properties)
        {
            var value = property.GetValue(exception.MessageParameter!, null)?.ToString();
            message = message.Replace($"{{{property.Name}}}", value);
        }

        yield return new ErrorResponseDescription(exception.Code, message);
    }
}
