using BRIX.Mobile.Resources.Localizations;
using System.Globalization;

namespace BRIX.Mobile.Services
{
    public interface ILocalizationResourceManager
    {
        public object this[string resourceKey] { get; }
        CultureInfo CurrentCulture { get; }
        List<CultureInfo> Cultures { get; }
        void SetCulture(CultureInfo culture);
    }

    public class LocalizationResourceManager : BindableObject, ILocalizationResourceManager
    {
        public LocalizationResourceManager()
        {
            string cultureName = Preferences.Get(Settings.Account.Culture, CultureInfo.CurrentCulture.Name);
            CultureInfo fromSettings = Cultures.FirstOrDefault(culture => culture == CultureInfo.GetCultureInfo(cultureName));
            Localization.Culture = fromSettings ?? Cultures.First();
        }

        public object this[string resourceKey] => Localization.ResourceManager.GetObject(resourceKey, Localization.Culture) ?? Array.Empty<byte>();

        public CultureInfo CurrentCulture => Localization.Culture;

        public List<CultureInfo> Cultures { get; } = new()
        {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("ru-RU"),
        };

        public void SetCulture(CultureInfo culture)
        {
            Localization.Culture = culture;
            OnPropertyChanged(null);
        }
    }
}
