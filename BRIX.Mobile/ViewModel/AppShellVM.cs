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

        public string Version { get; set; } = "Ver. " + VersionTracking.Default.CurrentVersion.ToString();
    }

    public class ShowCharacterTabsChanged(bool show) : ValueChangedMessage<bool>(show) { }
}
