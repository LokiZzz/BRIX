﻿using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Aspects;
using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;
using BRIX.Library.Ability;

namespace BRIX.Library.Effects
{
    public class HealEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects => new()
        {
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        };

        public override int BaseExpCost()
        {
            return (Impact.Average() * Impact.Average() * 1.2).Round();
        }
    }
}
