using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Services
{
    public static class LibraryTextHelper
    {
        private static ILocalizationResourceManager _localization => ServicePool.GetService<ILocalizationResourceManager>();

        private static Dictionary<Type, string> EffectNamesLocalizationKeys => new Dictionary<Type, string>
        {
            { typeof(DamageEffectModel), LocalizationKeys.EffectDamage }
        };

        public static string GetEffectName(EffectModel effectModel)
        {
            string key = EffectNamesLocalizationKeys[effectModel.GetType()];

            return _localization[key].ToString();
        }
    }
}
