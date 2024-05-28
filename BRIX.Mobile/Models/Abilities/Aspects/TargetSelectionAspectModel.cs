using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Enums;
using BRIX.Library.Mathematics;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class TargetSelectionAspectModel : SpecificAspectModelBase<TargetSelectionAspect>
    {
        public TargetSelectionAspectModel(TargetSelectionAspect model) : base(model) 
        {
            Obstacles = ObstacleOptionHelper.GetOptions(Resolver.Resolve<ILocalizationResourceManager>());
        }

        public ETargetSelectionStrategy Strategy
        {
            get => Internal.Strategy;
            set 
            { 
                SetProperty(Internal.Strategy, value, Internal, 
                    (model, prop) => model.Strategy = prop);
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
            }
        }

        public int NTADistance
        {
            get => Internal.NTAD.DistanceInMeters;
            set 
            { 
                SetProperty(Internal.NTAD.DistanceInMeters, value, Internal, 
                    (model, prop) => model.NTAD.DistanceInMeters = prop); 
            }
        }

        public ObstacleOptionVM? ObstacleBetweenCharacterAndTarget
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.NTAD.ObstacleBetweenCharacterAndTarget;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                if (value != null)
                {
                    EObstacleEquivalent equivalent = value.Equivalent;
                    SetProperty(Internal.NTAD.ObstacleBetweenCharacterAndTarget, equivalent, Internal,
                        (model, prop) => model.NTAD.ObstacleBetweenCharacterAndTarget = prop);
                }
            }
        }

        public bool IsRandomSelection
        {
            get => Internal.NTAD.IsTargetSelectionIsRandom;
            set
            {
                SetProperty(Internal.NTAD.IsTargetSelectionIsRandom, value, Internal,
                    (model, prop) => model.NTAD.IsTargetSelectionIsRandom = prop);
            }
        }

        #endregion

        #region Area

        public EAreaType AreaType
        {
            get => Internal.AreaSettings.Area.ShapeType;
            set
            {
                SetProperty(Internal.AreaSettings.Area.ShapeType, value, Internal, 
                    (model, prop) => model.AreaSettings.Area.ShapeType = prop);
                OnShapeChanged(value);
            }
        }

        public int AreaDistance
        {
            get => Internal.AreaSettings.DistanceToAreaInMeters;
            set
            {
                SetProperty(Internal.AreaSettings.DistanceToAreaInMeters, value, Internal, 
                    (model, prop) => model.AreaSettings.DistanceToAreaInMeters = prop);
            }
        }

        public int ExcludedTargetsCount
        {
            get => Internal.AreaSettings.ExcludedTargetsCount;
            set
            {
                SetProperty(Internal.AreaSettings.ExcludedTargetsCount, value, Internal,
                    (model, prop) => model.AreaSettings.ExcludedTargetsCount = prop);
            }
        }

        public bool IsAreaBoundedTo
        {
            get => Internal.AreaSettings.IsAreaBoundedTo;
            set
            {
                SetProperty(Internal.AreaSettings.IsAreaBoundedTo, value, Internal,
                    (model, prop) => model.AreaSettings.IsAreaBoundedTo = prop);
            }
        }

        public int R
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        return Internal.AreaSettings.Area.GetConcreteShape<Sphere>().R;
                    case EAreaType.Cylinder:
                        return Internal.AreaSettings.Area.GetConcreteShape<Cylinder>().R;
                    case EAreaType.Cone:
                        return Internal.AreaSettings.Area.GetConcreteShape<Cone>().R;
                    default:
                        return 1;
                }
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Sphere:
                        Sphere sphere = Internal.AreaSettings.Area.GetConcreteShape<Sphere>();
                        SetProperty(sphere.R, value, sphere, (model, prop) => model.R = prop);
                        break;
                    case EAreaType.Cylinder:
                        Cylinder cylinder = Internal.AreaSettings.Area.GetConcreteShape<Cylinder>();
                        SetProperty(cylinder.R, value, cylinder, (model, prop) => model.R = prop);
                        break;
                    case EAreaType.Cone:
                        Cone cone = Internal.AreaSettings.Area.GetConcreteShape<Cone>();
                        SetProperty(cone.R, value, cone, (model, prop) => model.R = prop);
                        break;
                }
            }
        }

        public int H
        {
            get
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        return Internal.AreaSettings.Area.GetConcreteShape<Cylinder>().H;
                    case EAreaType.Cone:
                        return Internal.AreaSettings.Area.GetConcreteShape<Cone>().H;
                    default:
                        return 1;
                }
            }
            set
            {
                switch (AreaType)
                {
                    case EAreaType.Cylinder:
                        Cylinder cylinder = Internal.AreaSettings.Area.GetConcreteShape<Cylinder>();
                        SetProperty(cylinder.H, value, cylinder, (model, prop) => model.H = prop);
                        break;
                    case EAreaType.Cone:
                        Cone cone = Internal.AreaSettings.Area.GetConcreteShape<Cone>();
                        SetProperty(cone.H, value, cone, (model, prop) => model.H = prop);
                        break;
                }
            }
        }

        public int A
        {
            get => Internal.AreaSettings.Area.Shape is Brick brick ? brick.A : 1;
            set
            {
                if(Internal.AreaSettings.Area.Shape is Brick brick)
                {
                    brick.A = value;
                }
            }
        }

        public int B
        {
            get => Internal.AreaSettings.Area.Shape is Brick brick ? brick.B : 1;
            set
            {
                if (Internal.AreaSettings.Area.Shape is Brick brick)
                {
                    brick.B = value;
                }
            }
        }

        public int C
        {
            get => Internal.AreaSettings.Area.Shape is Brick brick ? brick.C : 1;
            set
            {
                if (Internal.AreaSettings.Area.Shape is Brick brick)
                {
                    brick.C = value;
                }
            }
        }

        public int N
        {
            get => Internal.AreaSettings.Area.Shape is VoxelArray voxels ? voxels.N : 1;
            set
            {
                if (Internal.AreaSettings.Area.Shape is VoxelArray voxels)
                {
                    voxels.N = value;
                }
            }
        }

        private void OnShapeChanged(EAreaType shape)
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

        public ObstacleOptionVM? ObstacleBetweenCharacterAndArea
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.AreaSettings.ObstacleBetweenCharacterAndArea;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                if (value != null)
                {
                    EObstacleEquivalent equivalent = value.Equivalent;
                    SetProperty(Internal.AreaSettings.ObstacleBetweenCharacterAndArea, equivalent, Internal,
                        (model, prop) => model.AreaSettings.ObstacleBetweenCharacterAndArea = prop);
                }
            }
        }

        public ObstacleOptionVM? ObstacleBetweenEpicenterAndTarget
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.AreaSettings.ObstacleBetweenEpicenterAndTarget;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                if (value != null)
                {
                    EObstacleEquivalent equivalent = value.Equivalent;
                    SetProperty(Internal.AreaSettings.ObstacleBetweenEpicenterAndTarget, equivalent, Internal,
                        (model, prop) => model.AreaSettings.ObstacleBetweenEpicenterAndTarget = prop);
                }
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
            }
        }

        public int MaxDistanceBetweenTargetsInChain
        {
            get => Internal.TargetChain.MaxDistanceBetweenTargets;
            set
            {
                SetProperty(Internal.TargetChain.MaxDistanceBetweenTargets, value, Internal,
                    (model, prop) => model.TargetChain.MaxDistanceBetweenTargets = prop);
            }
        }

        public int MaxTargetsCountInChain
        {
            get => Internal.TargetChain.MaxTargetsCount;
            set
            {
                SetProperty(Internal.TargetChain.MaxTargetsCount, value, Internal,
                    (model, prop) => model.TargetChain.MaxTargetsCount = prop);
            }
        }

        public ObstacleOptionVM? ObstacleBetweenTargetsInChain
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.TargetChain.ObstacleBetweenTargetsInChain;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                if (value != null)
                {
                    EObstacleEquivalent equivalent = value.Equivalent;
                    SetProperty(Internal.TargetChain.ObstacleBetweenTargetsInChain, equivalent, Internal,
                        (model, prop) => model.TargetChain.ObstacleBetweenTargetsInChain = prop);
                }
            }
        }

        #endregion
    }
}