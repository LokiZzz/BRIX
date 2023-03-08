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
        private static ILocalizationResourceManager _localization => ServicePool.GetService<ILocalizationResourceManager>();

        public static Dictionary<Type, EffectUtilityModel> Collection => new()
        {
            { typeof(DamageEffectModel), new EffectUtilityModel() {
                Name = _localization[LocalizationKeys.EffectDamage].ToString(),
                Icon = AwesomeRPG.Sword,
                EditPage = typeof(HealDamageEffectPage)
            }},
            { typeof(HealEffect), new EffectUtilityModel() {
                Name = _localization[LocalizationKeys.EffectHeal].ToString(),
                Icon = AwesomeRPG.HealthIncrease,
                EditPage = typeof(HealDamageEffectPage) //временно
            }},
            { typeof(WinTheGameEffect), new EffectUtilityModel() {
                Name = "Just win",
                Icon = AwesomeRPG.FireballSword,
                EditPage = typeof(HealDamageEffectPage) //временно
            }},
            { typeof(DummyEffect), new EffectUtilityModel() {
                Name = "Win another way",
                Icon = AwesomeRPG.PoisonCloud,
                EditPage = typeof(HealDamageEffectPage) //временно
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
