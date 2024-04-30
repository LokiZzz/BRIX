using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AbilityActivationSettingsPageVM : ViewModelBase, IQueryAttributable
    {
        private AbilityActivationModel _activation = new();
        public AbilityActivationModel Activation
        {
            get => _activation;
            set => SetProperty(ref _activation, value);
        }

        private AbilityCostMonitorPanelVM? _costMonitor;
        public AbilityCostMonitorPanelVM? CostMonitor
        {
            get => _costMonitor;
            set => SetProperty(ref _costMonitor, value);
        }

        [RelayCommand]
        private void SetPoints(string points)
        {
            int intPoints = int.Parse(points);
            Activation.ActionPoints = intPoints;
            CostMonitor?.UpdateCost();
        }

        [RelayCommand]
        public async Task Save()
        {
            if (CostMonitor?.Ability?.Activation != null)
            {
                await Navigation.Back(
                    stepsBack: 1,
                    (NavigationParameters.AbilityActivation, CostMonitor.Ability.Activation)
                );
            }
            else
            {
                throw new Exception("Настройки активации не инициализированы.");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CostMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor)
                ?? throw new Exception("Монитор стоимости не передан в качестве параметра навигации.");
            
            CostMonitor.SaveCommand = SaveCommand;
            Activation = CostMonitor.Ability?.Activation 
                ?? throw new Exception("Модель способности в мониторе стоимости не инициализрована.");
        }
    }
}
