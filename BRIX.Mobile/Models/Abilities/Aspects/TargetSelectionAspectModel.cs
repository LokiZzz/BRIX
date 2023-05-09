using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Enums;
using BRIX.Library.Mathematics;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class TargetSelectionAspectModel : AspectModelBase
    {
        public TargetSelectionAspectModel(AspectBase model) : base(model) 
        {
            Obstacles = ObstacleOptionHelper.GetOptions(ServicePool.GetService<ILocalizationResourceManager>());
        }

        public TargetSelectionAspect Internal => GetSpecificAspect<TargetSelectionAspect>();

        public ETargetSelectionStrategy Strategy
        {
            get => Internal.Strategy;
            set 
            { 
                SetProperty(Internal.Strategy, value, Internal, 
                    (model, prop) => model.Strategy = prop);
                UpdateCost();
            }
        }

        private ObservableCollection<ObstacleOptionVM> _obstacles = new();
        public ObservableCollection<ObstacleOptionVM> Obstacles
        {
            get => _obstacles;
            set
            {
                SetProperty(ref _obstacles, value);
                OnPropertyChanged(nameof(ObstacleBetweenCharacterAndTarget));
                OnPropertyChanged(nameof(ObstacleBetweenCharacterAndArea));
                OnPropertyChanged(nameof(ObstacleBetweenEpicenterAndTarget));
                OnPropertyChanged(nameof(ObstacleBetweenTargetsInChain));
            }
        }

        #region NTAD

        public int NTADCount
        {
            get => Internal.NTAD.TargetsCount;
            set
            {
                SetProperty(Internal.NTAD.TargetsCount, value, Internal, 
                    (model, prop) => model.NTAD.TargetsCount = prop); 
                UpdateCost();
            }
        }

        public int NTADistance
        {
            get => Internal.NTAD.DistanceInMeters;
            set 
            { 
                SetProperty(Internal.NTAD.DistanceInMeters, value, Internal, 
                    (model, prop) => model.NTAD.DistanceInMeters = prop); 
                UpdateCost();
            }
        }

        public ObstacleOptionVM ObstacleBetweenCharacterAndTarget
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.NTAD.ObstacleBetweenCharacterAndTarget;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                EObstacleEquivalent equivalent = value.Equivalent;
                SetProperty(Internal.NTAD.ObstacleBetweenCharacterAndTarget, equivalent, Internal,
                    (model, prop) => model.NTAD.ObstacleBetweenCharacterAndTarget = prop);
                UpdateCost();
            }
        }

        public bool IsRandomSelection
        {
            get => Internal.NTAD.IsTargetSelectionIsRandom;
            set
            {
                SetProperty(Internal.NTAD.IsTargetSelectionIsRandom, value, Internal,
                    (model, prop) => model.NTAD.IsTargetSelectionIsRandom = prop);
                UpdateCost();
            }
        }

        #endregion

        #region Area

        public EAreaType AreaType
        {
            get => Internal.Area.AreaType;
            set
            {
                SetProperty(Internal.Area.AreaType, value, Internal, 
                    (model, prop) => model.Area.AreaType = prop);
                OnShapeParametersChanged(value);
                UpdateCost();
            }
        }

        public int AreaDistance
        {
            get => Internal.Area.DistanceToAreaInMeters;
            set
            {
                SetProperty(Internal.Area.DistanceToAreaInMeters, value, Internal, 
                    (model, prop) => model.Area.DistanceToAreaInMeters = prop);
                UpdateCost();
            }
        }

        public int ExcludedTargetsCount
        {
            get => Internal.Area.ExcludedTargetsCount;
            set
            {
                SetProperty(Internal.Area.ExcludedTargetsCount, value, Internal,
                    (model, prop) => model.Area.ExcludedTargetsCount = prop);
                UpdateCost();
            }
        }

        public int R
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        return ((Sphere)Internal.Area.Shape).R;
                    case EAreaType.Cylinder:
                        return ((Cylinder)Internal.Area.Shape).R;
                    case EAreaType.Cone:
                        return ((Cone)Internal.Area.Shape).R;
                    default:
                        return 1;
                }
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        Sphere sphere = (Sphere)Internal.Area.Shape;
                        SetProperty(sphere.R, value, sphere, (model, prop) => model.R = prop);
                        break;
                    case EAreaType.Cylinder:
                        Cylinder cylinder = (Cylinder)Internal.Area.Shape;
                        SetProperty(cylinder.R, value, cylinder, (model, prop) => model.R = prop);
                        break;
                    case EAreaType.Cone:
                        Cone cone = (Cone)Internal.Area.Shape;
                        SetProperty(cone.R, value, cone, (model, prop) => model.R = prop);
                        break;
                }
                
                UpdateCost();
            }
        }

        public int H
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        return ((Cylinder)Internal.Area.Shape).H;
                    case EAreaType.Cone:
                        return ((Cone)Internal.Area.Shape).H;
                    default:
                        return 1;
                }
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        Cylinder cylinder = (Cylinder)Internal.Area.Shape;
                        SetProperty(cylinder.H, value, cylinder, (model, prop) => model.H = prop);
                        break;
                    case EAreaType.Cone:
                        Cone cone = (Cone)Internal.Area.Shape;
                        SetProperty(cone.H, value, cone, (model, prop) => model.H = prop);
                        break;
                }

                UpdateCost();
            }
        }

        public int A
        {
            get => Internal.Area.Shape is Brick brick ? brick.A : 1;
            set
            {
                if(Internal.Area.Shape is Brick brick)
                {
                    brick.A = value;
                    UpdateCost();
                }
            }
        }

        public int B
        {
            get => Internal.Area.Shape is Brick brick ? brick.B : 1;
            set
            {
                if (Internal.Area.Shape is Brick brick)
                {
                    brick.B = value;
                    UpdateCost();
                }
            }
        }

        public int C
        {
            get => Internal.Area.Shape is Brick brick ? brick.C : 1;
            set
            {
                if (Internal.Area.Shape is Brick brick)
                {
                    brick.C = value;
                    UpdateCost();
                }
            }
        }

        public int N
        {
            get => Internal.Area.Shape is VoxelArray voxels ? voxels.N : 1;
            set
            {
                if (Internal.Area.Shape is VoxelArray voxels)
                {
                    voxels.N = value;
                    UpdateCost();
                }
            }
        }

        private void OnShapeParametersChanged(EAreaType shape)
        {
            switch (shape)
            {
                case EAreaType.Brick:
                    OnPropertyChanged(nameof(A));
                    OnPropertyChanged(nameof(B));
                    OnPropertyChanged(nameof(C));
                    break;
                case EAreaType.Sphere:
                    OnPropertyChanged(nameof(R));
                    break;
                case EAreaType.Cone:
                case EAreaType.Cylinder:
                    OnPropertyChanged(nameof(H));
                    OnPropertyChanged(nameof(R));
                    break;
                case EAreaType.Arbitrary:
                    OnPropertyChanged(nameof(N));
                    break;
            }
        }

        public ObstacleOptionVM ObstacleBetweenCharacterAndArea
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.Area.ObstacleBetweenCharacterAndArea;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                EObstacleEquivalent equivalent = value.Equivalent;
                SetProperty(Internal.Area.ObstacleBetweenCharacterAndArea, equivalent, Internal,
                    (model, prop) => model.Area.ObstacleBetweenCharacterAndArea = prop);
                UpdateCost();
            }
        }

        public ObstacleOptionVM ObstacleBetweenEpicenterAndTarget
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.Area.ObstacleBetweenEpicenterAndTarget;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                EObstacleEquivalent equivalent = value.Equivalent;
                SetProperty(Internal.Area.ObstacleBetweenEpicenterAndTarget, equivalent, Internal,
                    (model, prop) => model.Area.ObstacleBetweenEpicenterAndTarget = prop);
                UpdateCost();
            }
        }

        #endregion

        #region Target chain

        public bool IsChainEnabled
        {
            get => Internal.TargetChain.IsChainEnabled;
            set
            {
                SetProperty(Internal.TargetChain.IsChainEnabled, value, Internal,
                    (model, prop) => model.TargetChain.IsChainEnabled = prop);
                UpdateCost();
            }
        }

        public int MaxDistanceBetweenTargetsInChain
        {
            get => Internal.TargetChain.MaxDistanceBetweenTargets;
            set
            {
                SetProperty(Internal.TargetChain.MaxDistanceBetweenTargets, value, Internal,
                    (model, prop) => model.TargetChain.MaxDistanceBetweenTargets = prop);
                UpdateCost();
            }
        }

        public int MaxTargetsCountInChain
        {
            get => Internal.TargetChain.MaxTargetsCount;
            set
            {
                SetProperty(Internal.TargetChain.MaxTargetsCount, value, Internal,
                    (model, prop) => model.TargetChain.MaxTargetsCount = prop);
                UpdateCost();
            }
        }

        public ObstacleOptionVM ObstacleBetweenTargetsInChain
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.TargetChain.ObstacleBetweenTargetsInChain;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                EObstacleEquivalent equivalent = value.Equivalent;
                SetProperty(Internal.TargetChain.ObstacleBetweenTargetsInChain, equivalent, Internal,
                    (model, prop) => model.TargetChain.ObstacleBetweenTargetsInChain = prop);
                UpdateCost();
            }
        }

        #endregion
    }
}