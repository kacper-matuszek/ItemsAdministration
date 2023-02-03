using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters;

internal class ManyLocalizedExceptionResponseFormatter : BaseManyLocalizedExceptionResponseFormatter<BaseManyLocalizedException>
{
    internal ManyLocalizedExceptionResponseFormatter(
        ILogger<BaseManyLocalizedExceptionResponseFormatter<BaseManyLocalizedException>> logger, 
        ILocalizationService localizationService)
        : base(logger, localizationService)
    {
    }
}
