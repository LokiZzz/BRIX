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
                Name = Localization.AspectActionPoints,
                Icon = AwesomeRPG.Battery75,
                EditPage = typeof(ActionPointAspectPage),
                LibraryAspectType = typeof(ActionPointAspect),
            }},
            { typeof(TargetSelectionAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectTargetSelection,
                Icon = AwesomeRPG.ArcheryTarget,
                EditPage = typeof(TargetSelectionAspectPage),
                LibraryAspectType = typeof(TargetSelectionAspect),
            }},
            { typeof(CooldownAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectCooldown,
                Icon = AwesomeRPG.Hourglass,
                EditPage = typeof(CooldownAspectPage),
                LibraryAspectType = typeof(CooldownAspect),
            }},
            { typeof(ActivationConditionsAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectActivationConditions,
                Icon = AwesomeRPG.KeyBasic,
                EditPage = typeof(ActivationConditionsAspectPage),
                LibraryAspectType = typeof(ActivationConditionsAspect),
            }},
            { typeof(DurationAspectModel), new AspectUtilityModel() {
                Name = Localization.AspectRoundDuration,
                Icon = AwesomeRPG.Stopwatch,
                EditPage = typeof(DurationAspectPage),
                LibraryAspectType = typeof(DurationAspect),
            }},
        };

        public static string GetEditPageRoute(EffectBase effect)
        {
            return Collection[effect.GetType()].EditPage?.Name.ToString() ?? string.Empty;
        }
    }

    public class AspectUtilityModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Type? EditPage { get; set; }
        public Type? LibraryAspectType { get; set; }
    }
}
