using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class ChooseEffectPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;

        public ChooseEffectPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

        [ObservableProperty]
        private ObservableCollection<EffectToChooseVM> _effects;

        [RelayCommand]
        public async Task Choose(EffectToChooseVM effectToChoose)
        {
            await Navigation.NavigateAsync(
                effectToChoose.EditPage.Name,
                ENavigationMode.Push,
                (NavigationParameters.EditMode, EEditingMode.Add), 
                (NavigationParameters.Ability, _ability),
                (NavigationParameters.CostMonitor, _costMonitor)
            );
        }

        public override Task OnNavigatedAsync()
        {
            if (Effects != null) return Task.CompletedTask;

            Effects = new ObservableCollection<EffectToChooseVM>(
                EffectsDictionary.Collection.Select(x => 
                    new EffectToChooseVM() 
                    { 
                        Name = x.Value.Name, 
                        EditPage = x.Value.EditPage, 
                        Icon = x.Value.Icon 
                    }
                )
            );

            return Task.CompletedTask;
        }

        private AbilityModel _ability;
        private AbilityCostMonitorPanelVM _costMonitor;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability);
            _costMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);
            query.Clear();
        }
    }

    public class EffectToChooseVM
    {
        public string Name{ get; set; }
        public string Icon { get; set; }
        public Type EditPage { get; set; }
    }
}
