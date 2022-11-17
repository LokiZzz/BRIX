﻿using BRIX.Mobile.Resources.Localizations;
using System.Globalization;

namespace BRIX.Mobile.Services
{
    public interface ILocalizationResourceManager
    {
        public object this[string resourceKey] { get; }
        void SetCulture(CultureInfo culture);
    }

    public class LocalizationResourceManager : BindableObject, ILocalizationResourceManager
    {
        public LocalizationResourceManager()
        {
            string culture = Preferences.Get(Settings.Account.Culture, CultureInfo.CurrentCulture.Name);
            BrixApp.Culture = CultureInfo.GetCultureInfo(culture);
        }

        public object this[string resourceKey] => BrixApp.ResourceManager.GetObject(resourceKey, BrixApp.Culture) ?? Array.Empty<byte>();

        public void SetCulture(CultureInfo culture)
        {
            BrixApp.Culture = culture;
            OnPropertyChanged(null);
        }
    }
}
