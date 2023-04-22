using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities.Aspects;
using BRIX.Mobile.View.IconFonts;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public static class AspectsDictionary
    {
        private static ILocalizationResourceManager _localization => ServicePool.GetService<ILocalizationResourceManager>();

        public static Dictionary<Type, AspectUtilityModel> Collection => new()
        {
            { typeof(ActionPointsAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectActionPoints,
                Icon = AwesomeRPG.BottledBolt,
                EditPage = typeof(ActionPointAspectPage),
                LibraryAspectType = typeof(ActionPointAspect),
            }},
            { typeof(TargetSelectionAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectTargetSelection,
                Icon = AwesomeRPG.ArcheryTarget,
                EditPage = typeof(TargetSelectionAspectPage),
                LibraryAspectType = typeof(TargetSelectionAspect),
            }},
            { typeof(TargetChainAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectTargetChain,
                Icon = AwesomeRPG.PlayerThunderStruck,
                EditPage = typeof(TargetChainAspectPage),
                LibraryAspectType = typeof(TargetChainAspect),
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
