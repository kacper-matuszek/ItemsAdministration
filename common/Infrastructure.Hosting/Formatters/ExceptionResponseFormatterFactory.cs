using ItemsAdministration.Common.Infrastructure.Hosting.Formatters.Interfaces;
using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using ItemsAdministration.Common.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Formatters;

public sealed class ExceptionResponseFormatterFactory : IExceptionResponseFormatterFactory
{
    private readonly IServiceProvider _provider;

    public ExceptionResponseFormatterFactory(IServiceProvider provider) =>
        _provider = provider;

    public IExceptionResponseFormatter Create(Exception exception)
      => exception switch
      {
          BaseLocalizedException => new LocalizedExceptionResponseFormatter(GetLogger<LocalizedExceptionResponseFormatter>(), GetLocalizationService()),
          BaseManyLocalizedException => new ManyLocalizedExceptionResponseFormatter(GetLogger<ManyLocalizedExceptionResponseFormatter>(), GetLocalizationService()),
          _ => new DefaultExceptionResponseFormatter(GetLogger<DefaultExceptionResponseFormatter>(), GetLocalizationService())
      };

    private ILogger<TFormatter> GetLogger<TFormatter>() =>
        (ILogger<TFormatter>)_provider.GetService(typeof(ILogger<TFormatter>))!;

    private ILocalizationService GetLocalizationService() =>
        (ILocalizationService)_provider.GetService(typeof(ILocalizationService))!;
}
