using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    public class MoveTargetEffect : EffectBase
    {
        public override List<Type> RequiredAspects => new()
        {
                typeof(ActionPointAspect), typeof(TargetSelectionAspect), typeof(CooldownAspect), 
                typeof(ActivationConditionsAspect)
        };

        public int DistanceInMeters { get; set; }
        public EMoveTargetPath TargetPath { get; set; }
        public EMoveTargetDirectionRestriction DirectionRestriction { get; set; }

        public Dictionary<EMoveTargetPath, double> PathTypeToModifier = new()
        {
            { EMoveTargetPath.StraightBCaT, 0.8 },
            { EMoveTargetPath.Straight, 1 },
            { EMoveTargetPath.Arbitrary, 1.5 },
            { EMoveTargetPath.NoPath, 2 },
        };

        public Dictionary<EMoveTargetDirectionRestriction, double> DirectionRestrictionToModifier = new()
        {
            { EMoveTargetDirectionRestriction.OnlyHorizontalSurface, 1 },
            { EMoveTargetDirectionRestriction.Arbitrary, 2 },
        };

        public override int BaseExpCost()
        {
            ThrasholdCoefConverter thrasholdConverter = new(
                (1, 20), (2, 30), (3, 50), (4, 500), (7, 300), (21, 150), (101, 50)
            );

            int distanceExpCost = thrasholdConverter.Convert(DistanceInMeters);
            double pathTypeModifier = PathTypeToModifier[TargetPath];
            double directionRestrictionModifier = DirectionRestrictionToModifier[DirectionRestriction];

            return (distanceExpCost * pathTypeModifier * directionRestrictionModifier).Round();
        }
    }

    public enum EMoveTargetPath
    {
        /// <summary>
        /// По прямой между персонажем и его целью в любом направлении
        /// </summary>
        StraightBCaT = 1,
        /// <summary>
        /// По прямой
        /// </summary>
        Straight = 2,
        /// <summary>
        /// Произвольный
        /// </summary>
        Arbitrary = 3,
        /// <summary>
        /// Без пути, телепортация
        /// </summary>
        NoPath = 4
    }

    public enum EMoveTargetDirectionRestriction
    {
        /// <summary>
        /// Перммещение возможно только в горизонтальной плоскости
        /// </summary>
        OnlyHorizontalSurface = 1,
        /// <summary>
        /// Пермещение может производится в любых плоскостях
        /// </summary>
        Arbitrary = 2
    }
}
