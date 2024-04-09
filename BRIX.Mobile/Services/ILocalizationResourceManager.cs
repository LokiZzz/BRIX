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

        public object this[string resourceKey]
        {
            get 
            {
                object? resource = Localization.ResourceManager.GetObject(resourceKey, Localization.Culture);

                return resource  ?? Array.Empty<byte>();
            }
        }

        public List<string> GetKeys()
        {
            List<string> keys = [];
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
                return CurrentCulture.Name switch
                {
                    "en-US" => ELexisLanguage.English,
                    "ru-RU" => ELexisLanguage.Russian,
                    _ => ELexisLanguage.English,
                };
            }
        }

        public List<CultureInfo> Cultures { get; } =
        [
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("ru-RU"),
        ];

        public void SetCulture(CultureInfo culture)
        {
            Localization.Culture = culture;
            OnPropertyChanged(null);
        }
    }

    public static class MobileLexisProvider
    {
        public static string ToLexis(this object model)
        {
            return model.ToLexis(Localization.Culture).GetAwaiter().GetResult();
        }
    }
}
