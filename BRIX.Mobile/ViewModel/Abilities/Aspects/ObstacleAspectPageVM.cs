using BRIX.Library.Aspects;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ObstacleAspectPageVM : AspectPageVMBase<ObstacleAspectModel>
    {
        #region

        [ObservableProperty]
        public bool _betweenCharacterAndTarget;

        [ObservableProperty]
        public bool _betweenCharacterAndArea;

        [ObservableProperty]
        public bool _betweenEpicenterAndTarget;

        [ObservableProperty]
        public bool _betweenTargetsInChain;

        [ObservableProperty]
        public bool _betweenTargetAndDestinationPoint;

        #endregion

        #region Checkbox visibility

        [ObservableProperty]
        public bool _showNTADOptions;

        [ObservableProperty]
        public bool _showAreaOptions;

        [ObservableProperty]
        public bool _showChainOptions;

        [ObservableProperty]
        public bool _showMoveOptions;

        #endregion
    }
}
