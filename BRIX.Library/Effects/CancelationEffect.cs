using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects
{
    public class CancelationEffect : EffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        ];

        /// <summary>
        /// Максимальная мощность способности, которая может быть отменена.
        /// </summary>
        public int MaxAbilityPower { get; set; } = 100;

        public override int BaseExpCost() => MaxAbilityPower;
    }
}
