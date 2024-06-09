using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    public class MoveTargetEffect : EffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(TargetSelectionAspect), typeof(ActivationConditionsAspect)
        ];

        public int DistanceInMeters { get; set; } = 1;

        public EMoveTargetPath TargetPath { get; set; }

        public Dictionary<EMoveTargetPath, double> PathTypeToModifier = new()
        {
            { EMoveTargetPath.StraightToCharacter, 0.8 },
            { EMoveTargetPath.StraightFromCharacter, 0.8 },
            { EMoveTargetPath.Straight, 1 },
            { EMoveTargetPath.ArbitraryPath, 1.5 },
            { EMoveTargetPath.NoPath, 2 },
        };

        public override int BaseExpCost()
        {
            ThrasholdCostConverter thrasholdConverter = new(
                (1, 20), (2, 30), (3, 50), (4, 500), (7, 300), (21, 150), (101, 50)
            );

            int distanceExpCost = thrasholdConverter.Convert(DistanceInMeters);
            double pathTypeModifier = PathTypeToModifier[TargetPath];

            return (distanceExpCost * pathTypeModifier).Round();
        }
    }

    public enum EMoveTargetPath
    {
        /// <summary>
        /// По прямой по направлению к персонажу
        /// </summary>
        StraightToCharacter = 0,
        /// <summary>
        /// По прямой по направлению от персонажа
        /// </summary>
        StraightFromCharacter = 1,
        /// <summary>
        /// По прямой в любом направлении
        /// </summary>
        Straight = 2,
        /// <summary>
        /// По произвольному пути
        /// </summary>
        ArbitraryPath = 3,
        /// <summary>
        /// Без пути, телепортация
        /// </summary>
        NoPath = 4
    }
}
