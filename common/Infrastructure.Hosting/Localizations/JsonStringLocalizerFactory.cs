using ItemsAdministration.Common.Infrastructure.Readers.Interfaces;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Localizations;

internal sealed class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly ConcurrentDictionary<string, JsonStringLocalizer> _localizerCache =
        new ConcurrentDictionary<string, JsonStringLocalizer>();

    private readonly string _resourcesPath;
    private readonly IDictionaryJsonFileReader _jsonReader;

    public JsonStringLocalizerFactory(IOptions<LocalizationOptions> options, IDictionaryJsonFileReader jsonReader)
    {
        _resourcesPath = options.Value.ResourcesPath;
        _jsonReader = jsonReader;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return Create(resourceSource.FullName!, string.Empty);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        baseName = GetResourcePrefix(baseName, location);

        return _localizerCache.GetOrAdd(baseName, _ => CreateJsonStringLocalizer(baseName));
    }

    private string GetResourcePrefix(string baseName, string location) =>
        Path.Combine(location, _resourcesPath, baseName);
    private JsonStringLocalizer CreateJsonStringLocalizer(string baseName) =>
        new JsonStringLocalizer(baseName, _jsonReader);
}
