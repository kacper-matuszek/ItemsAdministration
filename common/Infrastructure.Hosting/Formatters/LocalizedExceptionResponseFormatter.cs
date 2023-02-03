using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters;

internal class LocalizedExceptionResponseFormatter : BaseLocalizedExceptionResponseFormatter<BaseLocalizedException>
{
    internal LocalizedExceptionResponseFormatter(
        ILogger<BaseLocalizedExceptionResponseFormatter<BaseLocalizedException>> logger,
        ILocalizationService localizationService) : base(logger, localizationService)
    {
    }
}
