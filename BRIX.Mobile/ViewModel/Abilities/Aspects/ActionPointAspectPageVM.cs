using BRIX.Library.DiceValue;
using BRIX.Library;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Library.Aspects;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ActionPointAspectPageVM : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private DamageEffectModel _damage = new();

        [ObservableProperty]
        private AbilityModel _ability = new();

        [ObservableProperty]
        private ActionPointsAspectModel _aspect = new();

        [ObservableProperty]
        private AbilityCostMonitorPanelVM _costMonitor;

        private int _actionPoints = 1;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (SetProperty(ref _actionPoints, value))
                {
                    Aspect.Internal.ActionPoints = value;
                    Ability.UpdateCost();
                }
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            await Navigation.Back(stepsBack: 1, (NavigationParameters.Aspect, Aspect));
        }

        [RelayCommand]
        private void SetPoints(string points)
        {
            int intPoints = int.Parse(points);
            ActionPoints = intPoints;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability) ?? new();
            Damage = query.GetParameterOrDefault<DamageEffectModel>(NavigationParameters.Effect) ?? new();
            Aspect = query.GetParameterOrDefault<ActionPointsAspectModel>(NavigationParameters.Aspect) ?? new();

            Damage.UpdateAspect(Aspect);
            Ability.UpdateEffect(Damage);

            query.Clear();
        }

        public override Task OnNavigatedAsync()
        {
            CostMonitor = new AbilityCostMonitorPanelVM(Ability, SaveCommand);
            Ability.UpdateCost();

            return Task.CompletedTask;
        }
    }
}
