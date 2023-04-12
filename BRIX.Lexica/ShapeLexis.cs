using BRIX.Lexica;
using BRIX.Library.Mathematics;

namespace BRIX.Lexis
{
    public class ShapeLexis
    {
        public static string GetLexis(IShape shape, ELexisLanguage language)
        {
            try
            {
                switch (language)
                {
                    case ELexisLanguage.Russian:
                        return GetDescriptionRUS(shape);
                    case ELexisLanguage.English:
                        return GetDescriptionENG(shape);
                    default:
                        return string.Empty;
                }
            }
            catch (NullReferenceException)
            {
                return string.Empty;
            }
        }

        private static string GetDescriptionRUS(IShape shape)
        {
            switch (shape)
            {
                case Brick brick:
                    return $"параллелепипед со сторонами {brick.A}, {brick.B} и {Numbers.RUSDeclension(brick.C, "метр")}";
                case Sphere sphere:
                    return $"сфера с радиусом {Numbers.RUSDeclension(sphere.R, "метр")}";
                case Cone cone:
                    return $"конус с радиусом {Numbers.RUSDeclension(cone.R, "метр")} и высотой {Numbers.RUSDeclension(cone.H, "метр")}";
                case Cylinder cylinder:
                    return $"цилиндр с радиусом {Numbers.RUSDeclension(cylinder.R, "метр")} и высотой {Numbers.RUSDeclension(cylinder.H, "метр")}";
                case VoxelArray voxels:
                    return $"массив из {Numbers.RUSDeclension(voxels.N, "воксель")} произвольной формы. Воксель — это куб 1х1 метр";
                default:
                    return string.Empty;
            }
        }

        private static string GetDescriptionENG(IShape shape)
        {
            switch (shape)
            {
                case Brick brick:
                    return $"parallelepiped with sides of {brick.A}, {brick.B} and {brick.C} meters";
                case Sphere sphere:
                    return $"sphere with a radius of {Numbers.ENGDeclension(sphere.R, "meter")}";
                case Cone cone:
                    return $"cone with a radius of {Numbers.ENGDeclension(cone.R, "meter")} and a height of " +
                        $"{Numbers.ENGDeclension(cone.H, "meter")}";
                case Cylinder cylinder:
                    return $"cylinder with a radius of {Numbers.ENGDeclension(cylinder.R, "meter")} " +
                        $" and a height of {Numbers.ENGDeclension(cylinder.H, "meter")}";
                case VoxelArray voxels:
                    return $"an arbitrary-shaped array of {Numbers.ENGDeclension(voxels.N, "voxel")}. Voxel is a 1x1 meter cube";
                default:
                    return string.Empty;
            }
        }
    }
}
