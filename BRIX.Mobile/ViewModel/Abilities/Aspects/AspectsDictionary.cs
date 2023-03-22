using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities.Aspects;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public static class AspectsDictionary
    {
        private static ILocalizationResourceManager _localization => ServicePool.GetService<ILocalizationResourceManager>();

        public static Dictionary<Type, AspectUtilityModel> Collection => new()
        {
            { typeof(ActionPointsAspectModel), new AspectUtilityModel() {
                Name = _localization[Resources.Localizations.LocalizationKeys.AspectActionPoints].ToString(),
                Icon = AwesomeRPG.BottledBolt,
                EditPage = typeof(ActionPointAspectPage),
                LibraryAspectType = typeof(ActionPointAspect),
            }},
        };

        public static string GetEditPageRoute(EffectBase effect)
        {
            return Collection[effect.GetType()].EditPage.Name.ToString();
        }
    }

    public class AspectUtilityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public Type EditPage { get; set; }
        public Type LibraryAspectType { get; set; }
    }
}
