using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;

namespace BRIX.Library.Effects
{
    public class PeriodicDamageEffect : DamageEffect
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), 
            typeof(DurationAspect), typeof(VampirismAspect)
        ];
    }
}
