using ItemsAdministration.Common.Infrastructure.Hosting.Descriptions;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;

internal class ManyLocalizedExceptionResponseFormatter<TException> : ExceptionResponseFormatter<TException>
    where TException : BaseManyLocalizedException
{
    protected ManyLocalizedExceptionResponseFormatter(
        ILogger<ManyLocalizedExceptionResponseFormatter<TException>> logger,
        ILocalizationService localizationService)
    : base(logger, localizationService)
    {
    }

    protected override IEnumerable<ErrorResponseDescription> GetErrorDescriptions(TException exception)
    {
        var uniqueErrors = new HashSet<ErrorResponseDescription>();

        foreach (var error in exception.Errors)
        {
            var message = LocalizationService.GetLocalizedString(error.Code).Value;

            if (error.MessageParameter is null)
            {
                uniqueErrors.Add(new ErrorResponseDescription(error.Code, message));
                continue;
            }

            var properties = error.MessageParameter!
                .GetType()
                .GetProperties(System.Reflection.BindingFlags.Public |
                               System.Reflection.BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(error.MessageParameter!, null)?.ToString();
                message = message.Replace($"{{{property.Name}}}", value);
            }

            uniqueErrors.Add(new ErrorResponseDescription(error.Code, message));
        }

        return uniqueErrors;
    }
}
