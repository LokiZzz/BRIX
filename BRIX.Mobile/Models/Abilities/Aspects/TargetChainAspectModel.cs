using BRIX.Library.Aspects;
using BRIX.Library.Enums;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public class TargetChainAspectModel : AspectModelBase
    {
        public TargetChainAspectModel(AspectBase model) : base(model) 
        {
            Obstacles = ObstacleOptionHelper.GetOptions(ServicePool.GetService<ILocalizationResourceManager>());
            OnPropertyChanged(nameof(ObstacleBetweenTargetsInChain));
        }

        public TargetChainAspect Internal => GetSpecificAspect<TargetChainAspect>();

        public bool IsEnabled
        {
            get => Internal.IsChainEnabled;
            set
            {
                SetProperty(Internal.IsChainEnabled, value, Internal, 
                    (model, prop) => model.IsChainEnabled = prop);
                UpdateCost();
            }
        }

        public int MaxDistanceBetweenTargets
        {
            get => Internal.MaxDistanceBetweenTargets;
            set
            {
                SetProperty(Internal.MaxDistanceBetweenTargets, value, Internal, 
                    (model, prop) => model.MaxDistanceBetweenTargets = prop);
                UpdateCost();
            }
        }

        public int MaxTargetsCount
        {
            get => Internal.MaxTargetsCount;
            set
            {
                SetProperty(Internal.MaxTargetsCount, value, Internal, 
                    (model, prop) => model.MaxTargetsCount = prop);
                UpdateCost();
            }
        }

        private ObservableCollection<ObstacleOptionVM> _obstacles = new();
        public ObservableCollection<ObstacleOptionVM> Obstacles
        {
            get => _obstacles;
            set => SetProperty(ref _obstacles, value);
        }

        public ObstacleOptionVM ObstacleBetweenTargetsInChain
        {
            get
            {
                EObstacleEquivalent equivalent = Internal.ObstacleBetweenTargetsInChain;

                return Obstacles.FirstOrDefault(x => x.Equivalent == equivalent);
            }
            set
            {
                EObstacleEquivalent equivalent = value.Equivalent;
                SetProperty(Internal.ObstacleBetweenTargetsInChain, equivalent, Internal,
                    (model, prop) => model.ObstacleBetweenTargetsInChain = prop);
                UpdateCost();
            }
        }
    }
}
