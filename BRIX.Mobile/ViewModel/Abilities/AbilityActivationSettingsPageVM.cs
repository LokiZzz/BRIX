using BRIX.Library.Abilities;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AbilityActivationSettingsPageVM(ILocalizationResourceManager localization) 
        : ViewModelBase, IQueryAttributable
    {
        public ILocalizationResourceManager Localization { get; } = localization;

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

        private ObservableCollection<TriggerOptionVM> _triggers = [];
        public ObservableCollection<TriggerOptionVM> Triggers
        {
            get => _triggers;
            set
            {
                SetProperty(ref _triggers, value);
                OnPropertyChanged(nameof(ShowNoTriggersText));
            }
        }

        public bool ShowNoTriggersText => Triggers?.Any() == false;

        [RelayCommand]
        public async Task AddTrigger()
        {
            List<object> allTriggers = Enum.GetValues<ETriggerProbability>()
                .Select(x => new TriggerOptionVM
                {
                    Probability = x,
                    Text = Localization["ETriggerProbability_" + x.ToString("G")].ToString() ?? string.Empty
                })
                .Select(x => x as object)
                .ToList();

            PickerPopupResult? result = await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(
                new()
                {
                    Title = Resources.Localizations.Localization.TriggerProbability,
                    Items = allTriggers,
                }
            );

            if (result != null)
            {
                if (result.SelectedItem is TriggerOptionVM concreteResult)
                {
                    EntryPopupResult? entryResult = await ShowPopupAsync<EntryPopup, EntryPopupResult, EntryPopupParameters>(
                        new EntryPopupParameters
                        {
                            Title = Resources.Localizations.Localization.ActivationTrigger,
                            Message = GetTriggerHint(concreteResult.Probability),
                            Placeholder = Resources.Localizations.Localization.Trigger,
                            ButtonText = Resources.Localizations.Localization.Ok,
                        }
                    );

                    if (entryResult == null)
                    {
                        return;
                    }

                    concreteResult.Text = entryResult.Text;
                    concreteResult = GetTriggerVM((concreteResult.Probability, concreteResult.Text));
                    Triggers.Add(concreteResult);
                    Activation.InternalModel.Triggers.Add((concreteResult.Probability, concreteResult.Text));
                    OnPropertyChanged(nameof(ShowNoTriggersText));
                }
            }

            Activation.CostMonitor?.UpdateCost();
        }

        [RelayCommand]
        public void DeleteTrigger(TriggerOptionVM property)
        {
            Triggers.Remove(property);

            (ETriggerProbability Probability, string Comment) triggerToDelete = Activation.InternalModel.Triggers
                .FirstOrDefault(x => x.Probability == property.Probability && x.Comment == property.Text);
            Activation.InternalModel.Triggers.Remove(triggerToDelete);

            Activation.CostMonitor?.UpdateCost();
            OnPropertyChanged(nameof(ShowNoTriggersText));
        }

        [RelayCommand]
        private void SetPoints(string points)
        {
            if (int.TryParse(points, out int actionPointsToSet))
            {
                Activation.ActionPoints = actionPointsToSet;
            }
        }

        private static string GetTriggerHint(ETriggerProbability probability)
        {
            return probability switch
            {
                ETriggerProbability.Low =>
                    Resources.Localizations.Localization.EnterLowProbabilityTrigger,
                ETriggerProbability.Standart =>
                    Resources.Localizations.Localization.EnterStandartProbabilityTrigger,
                ETriggerProbability.High =>
                    Resources.Localizations.Localization.EnterHighProbabilityTrigger,
                _ => string.Empty,
            };
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            AbilityCostMonitorPanelVM costMonitor =
                query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor)
                    ?? throw new Exception("Монитор стоимости не передан в качестве параметра навигации.");

            Activation = costMonitor?.Ability?.Activation
                ?? throw new Exception(" Настройки активации не инициализированы.");
            costMonitor.SaveCommand = SaveCommand;
            Activation.CostMonitor = costMonitor;

            Triggers = new(Activation.InternalModel.Triggers.Select(GetTriggerVM));
        }

        private TriggerOptionVM GetTriggerVM((ETriggerProbability Probability, string Comment) trigger)
        {
            string probabiltyText = Localization["ETriggerProbability_" + trigger.Probability.ToString("G")].ToString() 
                ?? string.Empty;
            string displayText = $"{probabiltyText}: «{trigger.Comment}»";

            return new TriggerOptionVM
            {
                Probability = trigger.Probability,
                Text = trigger.Comment,
                DisplayText = displayText
            };
        }
    }

    public class TriggerOptionVM
    {
        public ETriggerProbability Probability { get; set; }
        public string Text { get; set; } = string.Empty;

        public string DisplayText { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}
