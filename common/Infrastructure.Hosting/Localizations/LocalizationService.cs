using ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Localizations;

public sealed class LocalizationService : ILocalizationService
{
    private const string DefaultCulture = "en-US";

    private readonly IStringLocalizer _localizer;

    public LocalizationService(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    public LocalizedString GetLocalizedString(string key) =>
        _localizer.GetString(key);

    public LocalizedString GetLocalizedString(string key, string culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
            culture = DefaultCulture;

        var ci = new CultureInfo(culture);
        var currentCulture = CultureInfo.CurrentCulture;
        var currentUiCulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;

        var result = GetLocalizedString(key);

        CultureInfo.CurrentCulture = currentCulture;
        CultureInfo.CurrentUICulture = currentUiCulture;

        return result!;
    }

    public LocalizedString GetDefaultLocalizedString(string key) =>
        GetLocalizedString(key, DefaultCulture);
}
