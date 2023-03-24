using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AbilityCostMonitorPanelVM : ViewModelBase
    {
        public AbilityCostMonitorPanelVM(AbilityModel ability, IAsyncRelayCommand saveCommand)
        {
            Ability = ability;
            SaveCommand = saveCommand;
        }

        [ObservableProperty]
        private AbilityModel _ability;

        [ObservableProperty]
        private int _characterFreeExp;

        public IAsyncRelayCommand SaveCommand { get; }
    }
}
