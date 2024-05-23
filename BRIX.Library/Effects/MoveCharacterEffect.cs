using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    public class MoveCharacterEffect : EffectBase
    {
        public override List<Type> RequiredAspects => [typeof(ActivationConditionsAspect)];

        public Dictionary<ECharacterMovingType, double> MovingTypeToCostCoef = new()
        {
            { ECharacterMovingType.Walk, 1 },
            { ECharacterMovingType.Swim, 1 },
            { ECharacterMovingType.Climb, 1.5 },
            { ECharacterMovingType.Fly, 2 },
            { ECharacterMovingType.Burrow, 2 },
            { ECharacterMovingType.Teleportation, 4 },
        };

        public double DistancePerActionPoint { get; set; } = 2;

        public ECharacterMovingType MovingType { get; set; }

        public override int BaseExpCost()
        {
            // Обрубаем точность до сотых метра (сантиметров).
            int distanceInCm = (DistancePerActionPoint * 100).Round();
            int distanceBaseCost = new ThrasholdCostConverter((1, 1), (101, 5), (301, 10), (1001, 5))
                .Convert(distanceInCm);

            return (distanceBaseCost * MovingTypeToCostCoef[MovingType]).Round();
        }
    }

    public enum ECharacterMovingType
    {
        Walk,
        Swim,
        Climb,
        Fly,
        Burrow,
        Teleportation
    }
}
