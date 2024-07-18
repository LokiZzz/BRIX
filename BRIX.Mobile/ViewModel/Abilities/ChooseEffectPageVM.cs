using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.NPCs;
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
        private ObservableCollection<EffectTypeVM> _effects = [];

        [RelayCommand]
        public async Task Choose(EffectTypeVM effectToChoose)
        {
            if(effectToChoose.EditPage == null)
            {
                return;
            }

            await Navigation.NavigateAsync(
                effectToChoose.EditPage.Name,
                ENavigationMode.Push,
                (NavigationParameters.EditMode, EEditingMode.Add), 
                (NavigationParameters.CostMonitor, _costMonitor.Copy())
            );
        }

        public override Task OnNavigatedAsync()
        {
            if (Effects.Any()) return Task.CompletedTask;

            List<EffectTypeVM> effects = EffectsDictionary.Collection
                .Where(x => x.Value.Effect?.HasStatus == true || !_forStatus)
                .Select(x => x.Value)
                .ToList();
                
            if(_costMonitor?.Ability.Character != null && _costMonitor?.Ability.Character is NPC)
            {
                effects.Remove(effects.Single(x => x.Effect is SummonCreatureEffect));
            }

            Effects = new(effects);

            return Task.CompletedTask;
        }

        private AbilityCostMonitorPanelVM? _costMonitor;
        private bool _forStatus = false;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _costMonitor ??= query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);
            _forStatus = query.GetParameterOrDefault<bool>(NavigationParameters.ForStatus);

            query.Clear();
        }
    }
}
