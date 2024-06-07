using BRIX.Library.Enums;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Aspects.TargetSelection
{
    public class AreaSettings
    {
        public int DistanceToAreaInMeters { get; set; } = 1;
        public int ExcludedTargetsCount { get; set; } = 0;
        public EObstacleEquivalent ObstacleBetweenCharacterAndArea { get; set; } = EObstacleEquivalent.WoodenPlank;
        public EObstacleEquivalent ObstacleBetweenEpicenterAndTarget { get; set; } = EObstacleEquivalent.WoodenPlank;
        
        /// <summary>
        /// Привязывается ли зона в момент активации к персонажу или предмету.
        /// </summary>
        public bool IsAreaBoundedTo { get; set; } = false;

        public VolumeShape Area { get; set; } = new();
    }
}
