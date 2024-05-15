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
        public static Dictionary<Type, EffectTypeVM> Collection => new()
        {
            { typeof(EffectGenericModelBase<DamageEffect>), new EffectTypeVM() {
                Name = Localization.EffectDamage,
                Icon = AwesomeRPG.Sword,
                EditPage = typeof(DamageEffectPage)
            }},
            { typeof(EffectGenericModelBase<HealEffect>), new EffectTypeVM() {
                Name = Localization.EffectHeal,
                Icon = AwesomeRPG.HealthIncrease,
                EditPage = typeof(HealEffectPage)
            }},
            { typeof(EffectGenericModelBase<FortifyEffect>), new EffectTypeVM() {
                Name = Localization.EffectFortify,
                Icon = AwesomeRPG.HeartTower,
                EditPage = typeof(FortifyEffectPage),
                ForStatus = true
            }},
            { typeof(EffectGenericModelBase<ExhaustionEffect>), new EffectTypeVM() {
                Name = Localization.EffectExhaustion,
                Icon = AwesomeRPG.BleedingHearts,
                EditPage = typeof(ExhaustionEffectPage),
                ForStatus = true
            }},
            { typeof(EffectGenericModelBase<AccelerationEffect>), new EffectTypeVM() {
                Name = Localization.EffectAcceleration,
                Icon = AwesomeRPG.FeatherWing,
                EditPage = typeof(AccelerationEffectPage)
            }},
            { typeof(EffectGenericModelBase<DecelerationEffect>), new EffectTypeVM() {
                Name = Localization.EffectDeceleration,
                Icon = AwesomeRPG.Anchor,
                EditPage = typeof(DecelerationEffectPage)
            }},
            { typeof(EffectGenericModelBase<VulnerabilityEffect>), new EffectTypeVM() {
                Name = Localization.EffectVulnerability,
                Icon = AwesomeRPG.CrackedShield,
                EditPage = typeof(VulnerabilityEffectPage)
            }},
            { typeof(EffectGenericModelBase<DefenseEffect>), new EffectTypeVM() {
                Name = Localization.EffectDefense,
                Icon = AwesomeRPG.Shield,
                EditPage = typeof(DefenseEffectPage)
            }},
            { typeof(EffectGenericModelBase<AmplificationEffect>), new EffectTypeVM() {
                Name = Localization.EffectAmplification,
                Icon = AwesomeRPG.HammerDrop,
                EditPage = typeof(AmplificationEffectPage)
            }},
            { typeof(EffectGenericModelBase<ReductionEffect>), new EffectTypeVM() {
                Name = Localization.EffectReduction,
                Icon = AwesomeRPG.BleedingEye,
                EditPage = typeof(ReductionEffectPage)
            }},
            { typeof(CleanseEffectModel), new EffectTypeVM() {
                Name = Localization.EffectCleanse,
                Icon = AwesomeRPG.Aura,
                EditPage = typeof(CleanseEffectPage)
            }},
            { typeof(CancelationEffectModel), new EffectTypeVM() {
                Name = Localization.EffectCancelation,
                Icon = AwesomeRPG.Interdiction,
                EditPage = typeof(CancelationEffectPage)
            }},

            { typeof(EffectGenericModelBase<WinTheGameEffect>), new EffectTypeVM() {
                Name = Localization.EffectWin,
                Icon = AwesomeRPG.PerspectiveDiceRandom,
                EditPage = typeof(WinEffectPage)
            }},
            
        };

        public static string GetName(EffectBase effect)
        {
            EffectModelBase model = EffectModelFactory.GetModel(effect);

            return model != null 
                ? Collection[model.GetType()].Name
                : string.Empty;
        }

        public static string GetEditPageRoute(EffectModelBase effect)
        {
            return Collection[effect.GetType()].EditPage?.Name.ToString()
                ?? throw new Exception("Страница редактирования эффекта не надена в EffectsDictionary.");
        }
    }
}
