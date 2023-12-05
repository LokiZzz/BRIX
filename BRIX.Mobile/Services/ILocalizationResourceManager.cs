using BRIX.Lexica;
using BRIX.Mobile.Resources.Localizations;
using System.Collections;
using System.Globalization;

namespace BRIX.Mobile.Services
{
    public interface ILocalizationResourceManager
    {
        public object this[string resourceKey] { get; }
        CultureInfo CurrentCulture { get; }
        ELexisLanguage LexisLanguage { get; }
        List<CultureInfo> Cultures { get; }
        void SetCulture(CultureInfo culture);
        List<string> GetKeys();
    }

    public class LocalizationResourceManager : BindableObject, ILocalizationResourceManager
    {
        public LocalizationResourceManager()
        {
            string cultureName = Preferences.Get(Settings.Account.Culture, CultureInfo.CurrentCulture.Name);
            CultureInfo? fromSettings = Cultures.FirstOrDefault(culture => culture == CultureInfo.GetCultureInfo(cultureName));
            Localization.Culture = fromSettings ?? Cultures.First();
        }

        public object this[string resourceKey] => Localization.ResourceManager.GetObject(resourceKey, Localization.Culture) ?? Array.Empty<byte>();

        public List<string> GetKeys()
        {
            List<string> keys = new();
            System.Resources.ResourceSet? resourceSet = Localization.ResourceManager
                .GetResourceSet(CurrentCulture, false, false);

            if (resourceSet != null)
            {
                foreach (DictionaryEntry entry in resourceSet)
                {
                    string key = entry.Key.ToString() ?? string.Empty;

                    if (!string.IsNullOrEmpty(key))
                    {
                        keys.Add(key);
                    }
                }
            }

            return keys;
        }

        public CultureInfo CurrentCulture => Localization.Culture;

        public ELexisLanguage LexisLanguage
        {
            get
            {
                switch(CurrentCulture.Name)
                {
                    case "en-US": return ELexisLanguage.English;
                    case "ru-RU": return ELexisLanguage.Russian;
                    default: return ELexisLanguage.English;
                }
            }
        }

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
