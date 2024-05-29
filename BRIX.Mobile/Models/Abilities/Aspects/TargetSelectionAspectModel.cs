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
            AreaShape = new VolumeShapeModel(Internal.AreaSettings.Area);
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

        public VolumeShapeModel? _areaShape;
        public VolumeShapeModel? AreaShape
        {
            get => _areaShape;
            set
            {
                _areaShape = value;

                if (_areaShape != null)
                {
                    _areaShape.VolumeShapeChanged += (s, e) => UpdateCost();
                }
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