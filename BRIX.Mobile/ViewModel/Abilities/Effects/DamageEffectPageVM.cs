using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Mobile.Resources.Localizations;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class DamageEffectPageVM : DiceImpactEffectPageVMBase<DamageEffect>
    {
        protected override async Task<bool> Validate()
        {
            bool isValid = await base.Validate();
            Character? character = CostMonitor?.Character?.InternalModel;
            
            if(character != null && CostMonitor != null)
            {
                TargetSelectionAspect? tsa = Effect?.Internal.GetAspect<TargetSelectionAspect>();
                bool tooMuchSelfHarm = tsa != null
                    && tsa.Strategy == ETargetSelectionStrategy.CharacterHimself
                    && Effect?.Internal.Impact.Max() > character.MaxHealth;

                if (tooMuchSelfHarm)
                {
                    await Alert(Localization.TooMuchSelfharmMessage);
                    isValid = false;
                }

                bool moreThanOneSelfharmEffect = CostMonitor.Ability.Effects
                    .Where(x => 
                        x.InternalModel is DamageEffect dmg 
                        && dmg.GetAspect<TargetSelectionAspect>().Strategy == ETargetSelectionStrategy.CharacterHimself
                    ).Count() > 1;

                if(moreThanOneSelfharmEffect)
                {
                    await Alert(Localization.NoMoreThanOneSelfDamageEffectMessage);
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
