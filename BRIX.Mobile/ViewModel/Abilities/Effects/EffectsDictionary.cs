using BRIX.Library.Effects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Abilities.Effects;
using BRIX.Mobile.Models.Abilities.Effects;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public static class EffectsDictionary
    {
        public static Dictionary<Type, EffectUtilityModel> Collection => new()
        {
            { typeof(DamageEffectModel), new EffectUtilityModel() {
                Name = Localization.EffectDamage,
                Icon = AwesomeRPG.Sword,
                EditPage = typeof(DamageEffectPage)
            }},
            { typeof(HealEffectModel), new EffectUtilityModel() {
                Name = Localization.EffectHeal,
                Icon = AwesomeRPG.HealthIncrease,
                EditPage = typeof(HealEffectPage)
            }},
            { typeof(WinTheGameEffect), new EffectUtilityModel() {
                Name = "Just win",
                Icon = AwesomeRPG.FireballSword,
                EditPage = typeof(DamageEffectPage) //временно
            }},
        };

        public static string GetEditPageRoute(EffectModelBase effect)
        {
            return Collection[effect.GetType()].EditPage.Name.ToString();
        }
    }

    public class EffectUtilityModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Type EditPage { get; set; }
    }
}
