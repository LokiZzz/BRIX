using BRIX.Library.Effects.HealDamage;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.Base
{
    public abstract class MultiConditionalAspect<T> : AspectBase where T : Enum
    {
        public List<T> Conditions { get; set; } = new List<T>();

        public override double GetCoefficient()
        {
            if (!Conditions.Any())
            {
                return 1;
            }

            double coeficient = ((int)(object)Conditions.First()).ToCoeficient();

            foreach (T condition in Conditions.Skip(1))
            {
                coeficient *= ConditionToCoeficientMap[condition].ToCoeficient();
            }

            return coeficient;
        }

        public abstract Dictionary<T, int> ConditionToCoeficientMap { get; }
    }
}
