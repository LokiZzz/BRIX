using BRIX.Library.Enums;

namespace BRIX.Library.Aspects.TargetSelection
{
    /// <summary>
    /// Заданное количество уникальных целей на заданном расстоянии. 
    /// </summary>
    public class NTADSettings
    {
        public int TargetsCount { get; set; } = 1;
        public int DistanceInMeters { get; set; } = 1;
        public bool IsTargetSelectionIsRandom { get; set; }
        public EObstacleEquivalent ObstacleBetweenCharacterAndTarget { get; set; } = EObstacleEquivalent.WoodenPlank;
    }
}
