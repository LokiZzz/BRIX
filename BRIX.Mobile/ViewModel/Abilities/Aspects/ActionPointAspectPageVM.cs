using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.Models.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ActionPointAspectPageVM : AspectViewModelBase
    {
        [ObservableProperty]
        private DamageEffectModel _damage = new();

        [ObservableProperty]
        private ActionPointsAspectModel _aspect = new();

        private int _actionPoints = 1;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (SetProperty(ref _actionPoints, value))
                {
                    Aspect.Internal.ActionPoints = value;
                    CostMonitor.UpdateCost();
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

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CostMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);
            CostMonitor.SaveCommand = SaveCommand;
            Damage = query.GetParameterOrDefault<DamageEffectModel>(NavigationParameters.Effect) ?? new();
            Aspect = query.GetParameterOrDefault<ActionPointsAspectModel>(NavigationParameters.Aspect) ?? new();

            ActionPoints = Aspect.Internal.ActionPoints;

            Damage.UpdateAspect(Aspect);
            CostMonitor.Ability.UpdateEffect(Damage);

            query.Clear();
        }
    }
}
