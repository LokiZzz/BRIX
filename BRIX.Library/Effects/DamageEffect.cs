﻿using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Урон.
    /// </summary>
    public class DamageEffect : DiceImpactEffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect), typeof(VampirismAspect)
        ];

        public override int BaseExpCost()
        {
            int damageToHimselfCoef = 1;

            TargetSelectionAspect? targetSelection = GetAspect<TargetSelectionAspect>();

            if (targetSelection?.Strategy == ETargetSelectionStrategy.CharacterHimself)
            {
                damageToHimselfCoef = -1;
            }

            return Impact.Average() * Impact.Average() * damageToHimselfCoef;
        }
    }
}
