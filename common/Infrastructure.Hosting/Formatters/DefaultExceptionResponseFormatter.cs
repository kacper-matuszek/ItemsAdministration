using ItemsAdministration.Common.Infrastructure.Hosting.Descriptions;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters;

internal sealed class DefaultExceptionResponseFormatter : BaseExceptionResponseFormatter<Exception>
{
    private const string DefaultExceptionMessage = "An unexpected error occurred. Please contact system help desk.";

    public DefaultExceptionResponseFormatter(ILogger<DefaultExceptionResponseFormatter> logger, ILocalizationService localizationService)
         : base(logger, localizationService)
    {
    }

    protected override int GetStatusCode() => 500;

    protected override IEnumerable<ErrorResponseDescription> GetErrorDescriptions(Exception exception)
    {
        yield return new ErrorResponseDescription(string.Empty, DefaultExceptionMessage);
    }
}
