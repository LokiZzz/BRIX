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
                EditPage = typeof(DamageEffectPage), 
                Effect = new DamageEffect()
            }},
            { typeof(EffectGenericModelBase<HealEffect>), new EffectTypeVM() {
                Name = Localization.EffectHeal,
                Icon = AwesomeRPG.HealthIncrease,
                EditPage = typeof(HealEffectPage),
                Effect = new HealEffect()
            }},
            { typeof(EffectGenericModelBase<FortifyEffect>), new EffectTypeVM() {
                Name = Localization.EffectFortify,
                Icon = AwesomeRPG.HeartTower,
                EditPage = typeof(FortifyEffectPage),
                Effect = new FortifyEffect()
            }},
            { typeof(EffectGenericModelBase<ExhaustionEffect>), new EffectTypeVM() {
                Name = Localization.EffectExhaustion,
                Icon = AwesomeRPG.BleedingHearts,
                EditPage = typeof(ExhaustionEffectPage),
                Effect = new ExhaustionEffect()
            }},
            { typeof(EffectGenericModelBase<AccelerationEffect>), new EffectTypeVM() {
                Name = Localization.EffectAcceleration,
                Icon = AwesomeRPG.FeatherWing,
                EditPage = typeof(AccelerationEffectPage),
                Effect = new DamageEffect()
            }},
            { typeof(EffectGenericModelBase<DecelerationEffect>), new EffectTypeVM() {
                Name = Localization.EffectDeceleration,
                Icon = AwesomeRPG.Anchor,
                EditPage = typeof(DecelerationEffectPage),
                Effect = new DecelerationEffect()
            }},
            { typeof(EffectGenericModelBase<VulnerabilityEffect>), new EffectTypeVM() {
                Name = Localization.EffectVulnerability,
                Icon = AwesomeRPG.CrackedShield,
                EditPage = typeof(VulnerabilityEffectPage),
                Effect = new VulnerabilityEffect()
            }},
            { typeof(EffectGenericModelBase<DefenseEffect>), new EffectTypeVM() {
                Name = Localization.EffectDefense,
                Icon = AwesomeRPG.Shield,
                EditPage = typeof(DefenseEffectPage),
                Effect = new DefenseEffect()
            }},
            { typeof(EffectGenericModelBase<AmplificationEffect>), new EffectTypeVM() {
                Name = Localization.EffectAmplification,
                Icon = AwesomeRPG.HammerDrop,
                EditPage = typeof(AmplificationEffectPage),
                Effect = new AmplificationEffect()
            }},
            { typeof(EffectGenericModelBase<ReductionEffect>), new EffectTypeVM() {
                Name = Localization.EffectReduction,
                Icon = AwesomeRPG.BleedingEye,
                EditPage = typeof(ReductionEffectPage),
                Effect = new ReductionEffect()
            }},
            { typeof(CleanseEffectModel), new EffectTypeVM() {
                Name = Localization.EffectCleanse,
                Icon = AwesomeRPG.Aura,
                EditPage = typeof(CleanseEffectPage),
                Effect = new CleanseEffect()
            }},
            { typeof(CancelationEffectModel), new EffectTypeVM() {
                Name = Localization.EffectCancelation,
                Icon = AwesomeRPG.Interdiction,
                EditPage = typeof(CancelationEffectPage),
                Effect = new CancelationEffect()
            }},
            { typeof(MoveTargetEffectModel), new EffectTypeVM() {
                Name = Localization.EffectMoveTarget,
                Icon = AwesomeRPG.PlayerDodge,
                EditPage = typeof(MoveTargetEffectPage),
                Effect = new MoveTargetEffect()
            }},
            { typeof(MoveCharacterEffectModel), new EffectTypeVM() {
                Name = Localization.EffectMoveCharacter,
                Icon = AwesomeRPG.Footprint,
                EditPage = typeof(MoveCharacterEffectPage),
                Effect = new MoveCharacterEffect()
            }},
            { typeof(MoveAreaEffectModel), new EffectTypeVM() {
                Name = Localization.EffectMoveArea,
                Icon = AwesomeRPG.RadialBalance,
                EditPage = typeof(MoveAreaEffectPage),
                Effect = new MoveAreaEffect()
            }},
            { typeof(ShieldEffectModel), new EffectTypeVM() {
                Name = Localization.EffectShield,
                Icon = AwesomeRPG.CastleFlag,
                EditPage = typeof(ShieldEffectPage),
                Effect = new ShieldEffect()
            }},
            { typeof(EffectGenericModelBase<PeriodicDamageEffect>), new EffectTypeVM() {
                Name = Localization.EffectPeriodicDamage,
                Icon = AwesomeRPG.BleedingHearts,
                EditPage = typeof(PeriodicDamageEffectPage),
                Effect = new PeriodicDamageEffect()
            }},
            { typeof(EffectGenericModelBase<DifficultTerrainEffect>), new EffectTypeVM() {
                Name = Localization.EffectDifficultTerrain,
                Icon = AwesomeRPG.Lava,
                EditPage = typeof(DifficultTerrainEffectPage),
                Effect = new DifficultTerrainEffect()
            }},

            { typeof(EffectGenericModelBase<WinTheGameEffect>), new EffectTypeVM() {
                Name = Localization.EffectWin,
                Icon = AwesomeRPG.PerspectiveDiceRandom,
                EditPage = typeof(WinEffectPage),
                Effect = new WinTheGameEffect()
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
