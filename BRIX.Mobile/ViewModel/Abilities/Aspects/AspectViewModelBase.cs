using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public abstract partial class AspectViewModelBase : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private AbilityCostMonitorPanelVM _costMonitor;

        public abstract void ApplyQueryAttributes(IDictionary<string, object> query);
    }
}
