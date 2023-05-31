using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
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
        public static Dictionary<Type, AspectUtilityModel> Collection => new()
        {
            { typeof(ActionPointsAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectActionPoints.ToString(),
                Icon = AwesomeRPG.BottledBolt,
                EditPage = typeof(ActionPointAspectPage),
                LibraryAspectType = typeof(ActionPointAspect),
            }},
            { typeof(TargetSelectionAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectTargetSelection.ToString(),
                Icon = AwesomeRPG.ArcheryTarget,
                EditPage = typeof(TargetSelectionAspectPage),
                LibraryAspectType = typeof(TargetSelectionAspect),
            }},
            { typeof(CooldownAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectCooldown.ToString(),
                Icon = AwesomeRPG.Hourglass,
                EditPage = typeof(CooldownAspectPage),
                LibraryAspectType = typeof(CooldownAspect),
            }},
            { typeof(ActivationConditionsAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectActivationConditions.ToString(),
                Icon = AwesomeRPG.KeyBasic,
                EditPage = typeof(ActivationConditionsAspectPage),
                LibraryAspectType = typeof(ActivationConditionsAspect),
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
