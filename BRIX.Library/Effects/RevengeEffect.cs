using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Персонаж заявляет активацию и начинает накапливать входящий урон заданное количество раундов.
    /// До истечения этого времени персонаж может реакцией нанести весь накопленный урон противникам.
    /// Свойство Impact — это количество раундов.
    /// </summary>
    public class RevengeEffect : SinglePropEffectBase
    {
        public RevengeEffect()
        {
            Impact = 1;
        }

        public override bool HasStatus => true;

        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(VampirismAspect)
        ];

        public override int BaseExpCost() => Impact * 100;
    }
}
