using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    public class TargetSelectionAspect : AspectBase
    {
        public ETargetSelectionStrategy Strategy { get; set; } = ETargetSelectionStrategy.NTargetsAtDistanсeL;

        public double RandomSelectionCoef => NTAD?.IsTargetSelectionIsRandom == true ? 0.8 : 1;

        public override double GetCoefficient()
        {
            switch (Strategy)
            {
                case ETargetSelectionStrategy.Area:
                    return GetAreaCoeficient();
                case ETargetSelectionStrategy.NTargetsAtDistanсeL:
                    return GetNTADCoeficient();
                default:
                    return 1;
            }
        }

        public NTADSettings NTAD { get; set; } = new NTADSettings();

        private double GetNTADCoeficient() => GetNTADDistanceCoef() * GetNTADCountCoeficient() * RandomSelectionCoef;

        private double GetNTADDistanceCoef() => GetDistanceCoef(NTAD.DistanceInMeters);

        private double GetNTADCountCoeficient() => new ThrasholdCoefConverter((1, 0), (2, 100), (6, 50), (11, 10), (101, 1))
                .Convert(NTAD.TargetsCount)
                .ToCoeficient();

        public AreaSettings Area { get; set; } = new AreaSettings();

        private double GetAreaCoeficient() => 
            GetAreaDistanceCoeficient() 
            * GetAreaVolumeCoeficient()
            * GetExcludedTargetsCoef();

        private double GetAreaDistanceCoeficient() => GetDistanceCoef(Area.DistanceToAreaInMeters);

        private double GetAreaVolumeCoeficient() => (Area.Shape.GetVolume() * 5).ToCoeficient();

        private double GetDistanceCoef(int distance)
        {
            return new ThrasholdCoefConverter((1, 0), (2, 20), (3, 10), (21, 5), (101, 2), (1001, 1))
                .Convert(distance)
                .ToCoeficient();
        }

        private double GetExcludedTargetsCoef()
        {
            return new ThrasholdCoefConverter((0, 0), (1, 30), (6, 5))
                .Convert(Area.ExcludedTargetsCount)
                .ToCoeficient();
        }
    }

    public class NTADSettings
    {
        public int TargetsCount { get; set; } = 1;

        public int DistanceInMeters { get; set; } = 1;

        public bool IsTargetSelectionIsRandom { get; set; }
    }

    public class AreaSettings
    {
        public int DistanceToAreaInMeters { get; set; } = 0;

        public int ExcludedTargetsCount { get; set; } = 0;

        public EAreaType AreaType { get; set; } = EAreaType.Brick;

        public IShape? Shape
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Brick:
                        return _brick;
                    case EAreaType.Sphere:
                        return _sphere;
                    case EAreaType.Cylinder:
                        return _cylinder;
                    case EAreaType.Cone:
                        return _cone;
                    case EAreaType.Arbitrary:
                        return _voxelArray;
                    default:
                        return null;
                }
            }
        }

        private readonly Brick _brick = new Brick();
        private readonly Sphere _sphere = new Sphere();
        private readonly Cylinder _cylinder = new Cylinder();
        private readonly Cone _cone = new Cone();
        private readonly VoxelArray _voxelArray = new VoxelArray();

        public enum EAreaType
        {
            Brick = 0,
            Sphere = 1,
            Cylinder = 2,
            Cone = 3,
            Arbitrary = 4
        }
    }

    public enum ETargetSelectionStrategy
    {
        NTargetsAtDistanсeL = 0,
        Area = 1
    }
}
