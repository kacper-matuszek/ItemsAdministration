using ItemsAdministration.Common.Application.Abstractions.Exceptions;
using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Base;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using Microsoft.Extensions.Logging;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters;

internal class ObjectNotFoundExceptionResponseFormatter : BaseLocalizedExceptionResponseFormatter<ObjectNotFoundException>
{
    public ObjectNotFoundExceptionResponseFormatter(
        ILogger<BaseLocalizedExceptionResponseFormatter<ObjectNotFoundException>> logger,
        ILocalizationService localizationService)
        : base(logger, localizationService) { }

    protected override int GetStatusCode() => 404;
}