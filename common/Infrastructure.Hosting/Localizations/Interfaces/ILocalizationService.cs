using Microsoft.Extensions.Localization;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Localizations.Interfaces;

public interface ILocalizationService
{
    LocalizedString GetLocalizedString(string key);
    LocalizedString GetLocalizedString(string key, string culture);
    LocalizedString GetDefaultLocalizedString(string key);
}
