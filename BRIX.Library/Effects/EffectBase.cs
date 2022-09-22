﻿using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using System.Collections.ObjectModel;

namespace BRIX.Library.Effects
{
    public abstract class EffectBase
    {
        public ReadOnlyCollection<AspectBase> Aspects;

        public abstract int BaseExpCost();

        public int GetExpCost()
        {
            double resultingCost = BaseExpCost();

            foreach (AspectBase aspect in Aspects)
            {
                double coeficient = aspect.GetCoefficient();
                resultingCost = resultingCost * coeficient;
            }

            return resultingCost.Round();
        }

        public T GetAspect<T>() where T : AspectBase
        {
            AspectBase aspect = Aspects.FirstOrDefault(x => x is T);

            if (aspect == null)
            {
                throw new ArgumentException($"У эффекта {GetType()} нет аспекта {typeof(T)}");
            }

            return (T)aspect;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType();
        }
    }
}