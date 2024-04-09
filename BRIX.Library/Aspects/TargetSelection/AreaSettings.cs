using BRIX.Library.Enums;
using BRIX.Library.Mathematics;
using System.Net;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class AreaSettings
    {
        private readonly Brick _brick = new();
        private readonly Sphere _sphere = new();
        private readonly Cylinder _cylinder = new();
        private readonly Cone _cone = new();
        private readonly VoxelArray _voxelArray = new();

        public int DistanceToAreaInMeters { get; set; } = 0;
        public int ExcludedTargetsCount { get; set; } = 0;
        public EAreaType AreaType { get; set; } = EAreaType.Brick;
        public EObstacleEquivalent ObstacleBetweenCharacterAndArea { get; set; } = EObstacleEquivalent.WoodenPlank;
        public EObstacleEquivalent ObstacleBetweenEpicenterAndTarget { get; set; } = EObstacleEquivalent.WoodenPlank;
        
        /// <summary>
        /// Привязывается ли зона в момент активации к персонажу.
        /// </summary>
        public bool IsAreaBoundedTo { get; set; } = false;

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

        public T GetConcreteShape<T>() where T : class, IShape
        {
            return Shape as T ?? throw new Exception("Фигура не инициализирована");
        }
    }
}
