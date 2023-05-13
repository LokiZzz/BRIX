using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public abstract class MultiConditionalAspect<T> : AspectBase where T : Enum
    {
        public List<(T Type, string Comment)> Conditions { get; set; } = new ();

        public override double GetCoefficient()
        {
            if (!Conditions.Any())
            {
                return 1;
            }

            T restriction = (T)(object)Conditions.First().Type;
            double coeficient = ConditionToCoeficientMap[restriction].ToCoeficient();

            foreach ((T Type, string Comment) condition in Conditions.Skip(1))
            {
                coeficient *= ConditionToCoeficientMap[condition.Type].ToCoeficient();
            }

            return coeficient;
        }

        public abstract Dictionary<T, int> ConditionToCoeficientMap { get; }
    }
}
