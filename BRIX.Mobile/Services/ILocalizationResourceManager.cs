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
            string culture = Preferences.Get(Settings.Account.Culture, CultureInfo.CurrentCulture.Name);
            Localization.Culture = CultureInfo.GetCultureInfo(culture);
        }

        public object this[string resourceKey] => Localization.ResourceManager.GetObject(resourceKey, Localization.Culture) ?? Array.Empty<byte>();

        public CultureInfo CurrentCulture => Localization.Culture;

        public List<CultureInfo> Cultures { get; } = new()
        {
            CultureInfo.GetCultureInfo("ru"),
            CultureInfo.GetCultureInfo("en")
        };

        public void SetCulture(CultureInfo culture)
        {
            Localization.Culture = culture;
            OnPropertyChanged(null);
        }
    }
}
