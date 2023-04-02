using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
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
