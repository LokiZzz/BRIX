using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Library.Mathematics
{
    public class VolumeShape
    {
        private readonly Brick _brick = new();
        private readonly Sphere _sphere = new();
        private readonly Cylinder _cylinder = new();
        private readonly Cone _cone = new();
        private readonly VoxelArray _voxelArray = new();

        public EAreaType ShapeType { get; set; } = EAreaType.Brick;

        public IShape? Shape
        {
            get
            {
                switch (ShapeType)
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
