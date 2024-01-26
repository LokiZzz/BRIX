using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Mathematics;
using System;

namespace BRIX.Library.Aspects
{
    public abstract class MultiConditionalAspect : AspectBase
    {
        public List<(Enum Type, string Comment)> Conditions { get; set; } = [];

        public override double GetCoefficient()
        {
            if (!Conditions.Any())
            {
                return 1;
            }

            Enum restriction = (Enum)(object)Conditions.First().Type;
            double coeficient = ConditionToCoeficientMap[restriction].ToCoeficient();

            foreach ((Enum Type, string Comment) condition in Conditions.Skip(1))
            {
                coeficient *= ConditionToCoeficientMap[condition.Type].ToCoeficient();
            }

            return coeficient;
        }

        public abstract Dictionary<Enum, int> ConditionToCoeficientMap { get; }
    }
}
