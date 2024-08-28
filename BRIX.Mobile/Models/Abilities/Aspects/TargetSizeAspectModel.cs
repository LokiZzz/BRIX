using BRIX.Library.Aspects;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public partial class TargetSizeAspectModel(TargetSizeAspect model) : SpecificAspectModelBase<TargetSizeAspect>(model)
    {
        private ObservableCollection<TargetSizeVM> _sizes = [];
        public ObservableCollection<TargetSizeVM> Sizes
        {
            get => _sizes;
            set => SetProperty(ref _sizes, value);
        }
    }
}