using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class ChooseEffectPageVM : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private ObservableCollection<EffectTypeVM> _effects;

        [RelayCommand]
        public async Task Choose(EffectTypeVM effectToChoose)
        {
            await Navigation.NavigateAsync(
                effectToChoose.EditPage.Name,
                ENavigationMode.Push,
                (NavigationParameters.EditMode, EEditingMode.Add), 
                (NavigationParameters.CostMonitor, _costMonitor.Copy())
            );
        }

        public override Task OnNavigatedAsync()
        {
            if (Effects != null) return Task.CompletedTask;

            IEnumerable<EffectTypeVM> effects = EffectsDictionary.Collection.Select(x => x.Value);
                
            if(_forStatus)
            {
                effects = effects.Where(x => x.ForStatus);
            }

            Effects = new(effects);

            return Task.CompletedTask;
        }

        private AbilityCostMonitorPanelVM _costMonitor;
        private bool _forStatus = false;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (_costMonitor == null)
            {
                _costMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);
            }

            _forStatus = query.GetParameterOrDefault<bool>(NavigationParameters.ForStatus);

            query.Clear();
        }
    }
}
