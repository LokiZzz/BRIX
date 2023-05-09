using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class AreaSettings
    {
        public int DistanceToAreaInMeters { get; set; } = 0;
        public int ExcludedTargetsCount { get; set; } = 0;
        public EAreaType AreaType { get; set; } = EAreaType.Brick;
        public EObstacleEquivalent ObstacleBetweenCharacterAndArea { get; set; } = EObstacleEquivalent.WoodenPlank;
        public EObstacleEquivalent ObstacleBetweenEpicenterAndTarget { get; set; } = EObstacleEquivalent.WoodenPlank;

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
    }
}
