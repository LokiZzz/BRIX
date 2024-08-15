using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Персонаж заявляет активацию и начинает накапливать входящий урон заданное количество раундов.
    /// До истечения этого времени персонаж может реакцией нанести весь накопленный урон противникам.
    /// Свойство Impact — это количество раундов.
    /// </summary>
    public class RevengeEffect : EffectBase
    {
        public override bool IsPositive => true;
        
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), 
            typeof(ActivationConditionsAspect), 
            typeof(VampirismAspect), 
            typeof(DurationAspect)
        ];

        public override int BaseExpCost() => 200;
    }
}
