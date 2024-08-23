using BRIX.Library.Abilities;
using BRIX.Library.Extensions;

namespace BRIX.Library.Items
{
    public class ArtifactFeature : Ability
    {
        public bool ConsumesArtifact { get; set; }

        public override int ExpCost() =>  (base.ExpCost() * (ConsumesArtifact ? 0.25 : 1)).Round();
    }
}
