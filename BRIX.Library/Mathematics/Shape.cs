namespace BRIX.Library.Mathematics
{
    public interface IShape
    {
        int GetVolume();
    }

    public class Brick : IShape
    {
        public int A { get; set; } = 1;
        public int B { get; set; } = 1;
        public int C { get; set; } = 1;

        public int GetVolume() => A * B * C;
    }

    public class Sphere : IShape
    {
        public int R { get; set; } = 1;

        public int GetVolume() => 4 * R * R * R; // 4/3 * 3.14 * R^3
    }

    public class Cylinder : IShape
    {
        public int R { get; set; } = 1;
        public int H { get; set; } = 1;

        public int GetVolume() => 3 * R * R * H;
    }
    public class Cone : IShape
    {
        public int R { get; set; } = 1;
        public int H { get; set; } = 1;

        public int GetVolume() => R * R * H;
    }

    public class VoxelArray : IShape
    {
        public int N { get; set; } = 1;

        public int GetVolume() => N;
    }
}
