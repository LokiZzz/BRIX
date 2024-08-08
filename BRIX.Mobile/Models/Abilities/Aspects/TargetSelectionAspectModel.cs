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

        public bool NeedToSeeTarget
        {
            get => Internal.NeedToSeeTarget;
            set => SetProperty(Internal.NeedToSeeTarget, value, Internal, (model, prop) => model.NeedToSeeTarget = prop);
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

        public bool SpreadsAroundCorners
        {
            get => Internal.AreaSettings.SpreadsAroundCorners;
            set
            {
                SetProperty(Internal.AreaSettings.SpreadsAroundCorners, value, Internal,
                    (model, prop) => model.AreaSettings.SpreadsAroundCorners = prop);
            }
        }

        public VolumeShapeModel AreaShape { get; set; }

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

        #endregion
    }
}