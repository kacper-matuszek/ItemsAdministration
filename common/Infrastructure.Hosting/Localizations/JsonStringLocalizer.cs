using ItemsAdministration.Common.Infrastructure.Readers.Interfaces;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Localizations;

internal sealed class JsonStringLocalizer : IStringLocalizer
{
    private readonly ConcurrentDictionary<string, IReadOnlyDictionary<string, JsonElement>> _culturesCache =
        new ConcurrentDictionary<string, IReadOnlyDictionary<string, JsonElement>>();

    private readonly string _baseName;
    private readonly IDictionaryJsonFileReader _reader;
    private readonly CultureInfo? _culture;

    private CultureInfo CurrentCulture => _culture ?? CultureInfo.CurrentUICulture;

    public JsonStringLocalizer(string baseName, IDictionaryJsonFileReader reader)
    {
        _baseName = baseName;
        _reader = reader;
    }

    private JsonStringLocalizer(string baseName, IDictionaryJsonFileReader reader, CultureInfo culture)
        : this(baseName, reader)
    {
        _culture = culture;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetStringSafely(name);

            return new LocalizedString(name, value ?? name, value == null, _baseName);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetStringSafely(name);
            var value = string.Format(format ?? name, arguments);

            return new LocalizedString(name, value, format == null, _baseName);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var allNames = new List<string>();

        var current = CurrentCulture;
        while (true)
        {
            var json = GetCultureResource(current, false);
            allNames.AddRange(json.Keys);

            if (!includeParentCultures || current.Equals(CultureInfo.InvariantCulture))
                break;

            current = current.Parent;
        }

        return allNames
            .Distinct()
            .Select(name => this[name])
            .ToArray();
    }

    private IReadOnlyDictionary<string, JsonElement> GetCultureResource(CultureInfo culture, bool includeParentCultures)
    {
        return _culturesCache.GetOrAdd(culture.Name, _ =>
        {

            var result = _reader.Read(GetCultureResourceName(culture));
            if (result != null || !includeParentCultures || culture.Equals(CultureInfo.InvariantCulture))
                return result ?? new Dictionary<string, JsonElement>();

            return GetCultureResource(culture.Parent, true);
        });
    }

    private string GetCultureResourceName(CultureInfo culture) =>
        !string.IsNullOrEmpty(culture?.Name) ? $"{_baseName}.{culture.Name}" : _baseName;

    private string? GetStringSafely(string key) =>
        GetStringSafely(GetCultureResource(CurrentCulture, true), key);

    private static string? GetStringSafely(IReadOnlyDictionary<string, JsonElement> jObject, string key)
    {
        if (jObject.ContainsKey(key) && jObject[key].ValueKind == JsonValueKind.String)
            return jObject[key].GetString() ?? null;

        return null;
    }
}
