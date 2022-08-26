using BRIX.Library.Effects.HealDamage;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.Base
{
    public abstract class SingleConditionAspect<T> : AspectBase where T : Enum
    {
        public T Condition { get; set; }

        public abstract Dictionary<T, int> ConditionToCoeficientMap { get; }

        public override double GetCoefficient() => ConditionToCoeficientMap[Condition].ToCoeficient();
    }
}
