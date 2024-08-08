using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class AreaSettings
    {
        public int DistanceToAreaInMeters { get; set; } = 1;

        public int ExcludedTargetsCount { get; set; } = 0;
        
        /// <summary>
        /// Огибает ли область углы.
        /// </summary>
        public bool SpreadsAroundCorners { get; set; } = false;

        public VolumeShape Area { get; set; } = new();
    }
}
