using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Characters;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BRIX.Mobile.ViewModel
{
    public partial class AppShellVM : ViewModelBase 
    {
        public AppShellVM()
        {
            WeakReferenceMessenger.Default.Register<ShowCharacterTabsChanged>(
                this, 
                (r, m) => ShowCharacterTabs = m.Value
            );
        }

        [ObservableProperty]
        private bool _showCharacterTabs = false;
    }

    public class ShowCharacterTabsChanged : ValueChangedMessage<bool> 
    { 
        public ShowCharacterTabsChanged(bool show) : base(show) { } 
    }
}
