using BRIX.Library.Aspects;
using System.ComponentModel;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Создание сложной местности.
    /// </summary>
    public class DifficultTerrainEffect : SinglePropEffectBase
    {
        public DifficultTerrainEffect()
        {
            Impact = 2;
        }

        public override List<Type> RequiredAspects =>
        [
            typeof(AOEAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect),
        ];

        public override int BaseExpCost()
        {
            return Impact * 10;
        }
    }
}
