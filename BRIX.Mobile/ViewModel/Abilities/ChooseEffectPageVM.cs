﻿using BRIX.Mobile.Models.Abilities;
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

            IEnumerable<EffectTypeVM> effects = _forStatus
                ? EffectsDictionary.Collection.Where(x => x.Value.Effect?.HasStatus == true).Select(x => x.Value)
                : EffectsDictionary.Collection.Select(x => x.Value);
                
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
