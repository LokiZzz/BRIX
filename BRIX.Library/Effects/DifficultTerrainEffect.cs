﻿using BRIX.Library.Aspects;
using System.ComponentModel;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Создание сложной местности.
    /// </summary>
    public class DifficultTerrainEffect : SinglePropEffectBase
    {
        public override bool IsPositive => false;

        public DifficultTerrainEffect()
        {
            Impact = 2;
        }

        public override List<Type> RequiredAspects =>
        [
            typeof(AOEAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect), typeof(TargetSizeAspect)
        ];

        public override int BaseExpCost()
        {
            return Impact * 10;
        }
    }
}
