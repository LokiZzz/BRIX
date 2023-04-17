using BRIX.Library.Aspects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ObstacleAspectPageVM : AspectPageVMBase<ObstacleAspectModel>
    {
        private readonly ILocalizationResourceManager _localization;

        public ObstacleAspectPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

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

        private ObservableCollection<ObstacleMaterialEquivalentVM> _equivalents = new();

        public ObservableCollection<ObstacleMaterialEquivalentVM> Equivalents
        {
            get => _equivalents;
            set => SetProperty(ref _equivalents, value);
        }

        public override void Initialize()
        {
            List<ObstacleMaterialEquivalentVM> equivalentsList = Enum.GetValues<EObstacleEquivalent>()
                .Select(GetLocalizedObstacleEquivalent)
                .ToList();
            Equivalents = new(equivalentsList);
        }

        private ObstacleMaterialEquivalentVM GetLocalizedObstacleEquivalent(EObstacleEquivalent equivalent)
        {
            return new ObstacleMaterialEquivalentVM()
            {
                Equivalent = equivalent,
                LocalizedText = _localization[equivalent.ToString("G")].ToString()
            };
        }
    }

    public class ObstacleMaterialEquivalentVM
    {
        public string LocalizedText { get; set; }
        public EObstacleEquivalent Equivalent { get; set; }
    }
}
