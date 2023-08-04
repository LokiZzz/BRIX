using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class ActionPointAspect : AspectBase
    {
        private int _minActionPoints = 1;
        private int _maxActionPoints = 50;

        private int _actionPoints = 1;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (value < _minActionPoints || _maxActionPoints < value)
                {
                    throw new ArgumentException("Способность может тратить от 1 до 50 очков действий.");
                }

                _actionPoints = value;
            }
        }

        public override double GetCoefficient()
        {
            int percent = new ThrasholdCostConverter((1, 0), (2, 20), (3, 10), (6, 1))
                .Convert(ActionPoints);
            double coef = (-percent).ToCoeficient();

            return coef;
        }
    }
}
