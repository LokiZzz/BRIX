using BRIX.Library.Abilities;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities
{
    public partial class AbilityActivationModel(AbilityActivation activation) : ObservableObject
    {
        public AbilityActivationModel() : this(new AbilityActivation()) { }

        public AbilityActivation InternalModel { get; private set; } = activation;

        private AbilityCostMonitorPanelVM? _costMonitor;
        public AbilityCostMonitorPanelVM? CostMonitor
        {
            get => _costMonitor;
            set => SetProperty(ref _costMonitor, value);
        }

        public int ActionPoints
        {
            get => InternalModel.ActionPoints;
            set
            {
                if(value < 1 || value > 25)
                {
                    return;
                }

                SetProperty(InternalModel.ActionPoints, value, InternalModel, (model, prop) => {
                    model.ActionPoints = prop;
                    CostMonitor?.UpdateCost();
                });
            }
        }

        public bool HasCooldown
        {
            get => InternalModel.UsesCountPerDay != 0;
            set
            {
                UsesCount = value ? 1 : 0;
            }
        }

        public int UsesCount
        {
            get => InternalModel.UsesCountPerDay;
            set
            {
                SetProperty(InternalModel.UsesCountPerDay, value, InternalModel, (model, prop) => {
                    model.UsesCountPerDay = prop;
                    CostMonitor?.UpdateCost();
                });
                OnPropertyChanged(nameof(HasCooldown));
            }
        }
    }
}
