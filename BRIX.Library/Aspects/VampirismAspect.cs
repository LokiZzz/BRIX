using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects
{
    /// <summary>
    /// Аспект, позволяющий восстанавливать здоровье 
    /// </summary>
    public class VampirismAspect : AspectBase
    {
        public int VampirismPercent { get; set; } = 0;

        public override double GetCoefficient()
        {
            return VampirismPercent.ToCoeficient();
        }
    }
}
