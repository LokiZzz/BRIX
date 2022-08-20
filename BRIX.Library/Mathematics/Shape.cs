using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Mathematics
{
    public interface IShape
    {
        int GetVolume();
    }

    public class Brick : IShape
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public int GetVolume() => A * B * C;
    }

    public class Sphere : IShape
    {
        public int R { get; set; }

        public int GetVolume() => 4 * R * R * R;
    }

    public class Cylinder : IShape
    {
        public int R { get; set; }
        public int H { get; set; }

        public int GetVolume() => 3 * R * R * H;
    }
    public class Cone : IShape
    {
        public int R { get; set; }
        public int H { get; set; }

        public int GetVolume() => R * R * H;
    }

    public class VoxelArray : IShape
    {
        public int N { get; set; }

        public int GetVolume() => N;
    }
}
