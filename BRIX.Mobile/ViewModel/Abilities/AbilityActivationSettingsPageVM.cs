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

        [RelayCommand]
        public async Task Save()
        {
            if (Activation != null)
            {
                await Navigation.Back(
                    stepsBack: 1,
                    (NavigationParameters.AbilityActivation, Activation)
                );
            }
            else
            {
                throw new Exception("Настройки активации не инициализированы.");
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            AbilityCostMonitorPanelVM costMonitor = 
                query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor)
                    ?? throw new Exception("Монитор стоимости не передан в качестве параметра навигации.");

            Activation = costMonitor?.Ability?.Activation
                ?? throw new Exception(" Настройки активации не инициализированы."); ;
            costMonitor.SaveCommand = SaveCommand;
            Activation.CostMonitor = costMonitor;
        }
    }
}
