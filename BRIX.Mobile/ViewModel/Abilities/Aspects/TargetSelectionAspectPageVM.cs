using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class TargetSelectionAspectPageVM : AspectPageVMBase<TargetSelectionAspectModel>
    {
        [ObservableProperty]
        private bool _isNTAD = true;

        [ObservableProperty]
        private bool _isAREA = false;

        [ObservableProperty]
        private int _NTADTargetsCount = 1;

        [ObservableProperty]
        private int _NTADDistance = 1;

        [RelayCommand]
        public void SetNTAD()
        {
            IsAREA = false;
            IsNTAD = true;
        }

        [RelayCommand]
        public void SetAREA()
        {
            IsNTAD = false;
            IsAREA = true;
        }

        public override void Initialize(IDictionary<string, object> query)
        {
        }
    }
}
