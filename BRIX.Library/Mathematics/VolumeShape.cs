using BRIX.Library.Aspects.TargetSelection;

namespace BRIX.Library.Mathematics
{
    public class VolumeShape
    {
        public readonly Brick Brick = new();
        public readonly Sphere Sphere = new();
        public readonly Cylinder Cylinder = new();
        public readonly Cone Cone = new();
        public readonly VoxelArray VoxelArray = new();

        public EAreaType ShapeType { get; set; } = EAreaType.Brick;

        public IShape Shape
        {
            get
            {
                switch (ShapeType)
                {
                    case EAreaType.Brick:
                        return Brick;
                    case EAreaType.Sphere:
                        return Sphere;
                    case EAreaType.Cylinder:
                        return Cylinder;
                    case EAreaType.Cone:
                        return Cone;
                    case EAreaType.VoxelArray:
                        return VoxelArray;
                    default:
                        throw new Exception($"Неконсистетное состояние модели {nameof(VolumeShape)}"); ;
                }
            }
        }

        public T GetConcreteShape<T>() where T : class, IShape
        {
            return Shape as T ?? throw new Exception("Фигура не инициализирована");
        }
    }
}
