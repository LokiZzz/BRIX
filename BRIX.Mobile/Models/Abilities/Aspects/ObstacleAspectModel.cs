using BRIX.Library.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class ObstacleAspectModel : AspectModelBase
    {
        public ObstacleAspectModel(AspectBase model) : base(model) { }

        public ObstacleAspect Internal => GetSpecificAspect<ObstacleAspect>();

        public EObstacleEquivalent BetweenCharacterAndTarget
        {
            get => Internal.BetweenCharacterAndTarget;
            set
            {
                SetProperty(Internal.BetweenCharacterAndTarget, value, Internal, 
                    (model, prop) => model.BetweenCharacterAndTarget = prop);
                UpdateCost();
            }
        }

        public EObstacleEquivalent BetweenCharacterAndArea
        {
            get => Internal.BetweenCharacterAndArea;
            set
            {
                SetProperty(Internal.BetweenCharacterAndArea, value, Internal,
                    (model, prop) => model.BetweenCharacterAndArea = prop);
                UpdateCost();
            }
        }

        public EObstacleEquivalent BetweenEpicenterAndTarget
        {
            get => Internal.BetweenEpicenterAndTarget;
            set
            {
                SetProperty(Internal.BetweenEpicenterAndTarget, value, Internal,
                    (model, prop) => model.BetweenEpicenterAndTarget = prop);
                UpdateCost();
            }
        }

        public EObstacleEquivalent BetweenTargetsInChain
        {
            get => Internal.BetweenTargetsInChain;
            set
            {
                SetProperty(Internal.BetweenTargetsInChain, value, Internal,
                    (model, prop) => model.BetweenTargetsInChain = prop);
                UpdateCost();
            }
        }

        public EObstacleEquivalent BetweenCharacterAndFinalMovigPoint
        {
            get => Internal.BetweenTargetAndDestinationPoint;
            set
            {
                SetProperty(Internal.BetweenTargetAndDestinationPoint, value, Internal,
                    (model, prop) => model.BetweenTargetAndDestinationPoint = prop);
                UpdateCost();
            }
        }
    }
}
