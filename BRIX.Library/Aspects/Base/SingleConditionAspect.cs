using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public abstract class SingleConditionAspect<T> : AspectBase where T : Enum
    {
        public T Condition { get; set; }

        public abstract Dictionary<T, int> ConditionToCoeficientMap { get; }

        public override double GetCoefficient() => ConditionToCoeficientMap[Condition].ToCoeficient();
    }
}
