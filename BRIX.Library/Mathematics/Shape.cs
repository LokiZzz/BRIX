using BRIX.Library.Extensions;

namespace BRIX.Library.Mathematics
{
    public interface IShape
    {
        double GetVolume();
    }

    public class Brick : IShape
    {
        public double A { get; set; } = 1;
        public double B { get; set; } = 1;
        public double C { get; set; } = 1;

        public double GetVolume() => A * B * C;

        public override string ToString()
        {
            return $"Brick({A}x{B}x{C})";
        }
    }

    public class Sphere : IShape
    {
        public double R { get; set; } = 1;

        // 4/3 * 3.14 * R^3 * коэффициент для повышения эффективности неудобной области
        public double GetVolume() => (4 * R * R * R * 0.5).Round(); 
        
        public override string ToString()
        {
            return $"Sphere({R})";
        }
    }

    public class Cylinder : IShape
    {
        public double R { get; set; } = 1;
        public double H { get; set; } = 1;

        public double GetVolume()
        {
            return 3 * R * R * H;
        }

        public override string ToString()
        {
            return $"Cylinder({H} R{R})";
        }
    }
    public class Cone : IShape
    {
        public double R { get; set; } = 1;
        public double H { get; set; } = 1;

        // R^2 * H * коэффициент для повышения эффективности неудобной области
        public double GetVolume() => (R * R * H * 0.5).Round();

        public override string ToString()
        {
            return $"Cone({H} R{R})";
        }
    }

    public class VoxelArray : IShape
    {
        public int N { get; set; } = 1;

        /// <summary>
        /// Является ли массив вокселей произвольным.
        /// </summary>
        public bool IsArbitrary { get; set; }

        public double GetVolume() => N;

        public override string ToString()
        {
            string arbitrary = IsArbitrary ? ", arbitrary" : "";

            return $"Voxels({N}{arbitrary})";
        }
    }
}
