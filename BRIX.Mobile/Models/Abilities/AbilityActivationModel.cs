using BRIX.Library.Abilities;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities
{
    public partial class AbilityActivationModel : ObservableObject
    {
        public AbilityActivationModel() : this(new AbilityActivation()) { }

        public AbilityActivationModel(AbilityActivation activation)
        {
            InternalModel = activation;

            ILocalizationResourceManager localization = Resolver.Resolve<ILocalizationResourceManager>();

            CooldownOptions = new(Enum.GetValues<ECooldownOption>().Select(x => new CooldownOptionVM
            {
                Cooldown = x,
                LocalizedName = localization[x.ToString("G")].ToString() ?? string.Empty,
            }));
        }

        public AbilityActivation InternalModel { get; private set; }

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
                SetProperty(InternalModel.ActionPoints, value, InternalModel, (model, prop) => {
                    model.ActionPoints = prop;
                    CostMonitor?.UpdateCost();
                });
            }
        }

        public CooldownOptionVM? SelectedCooldownOption
        {
            get
            {
                ECooldownOption cooldown = InternalModel.Cooldown;

                return CooldownOptions.FirstOrDefault(x => x.Cooldown == cooldown);
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                ECooldownOption cooldown = value.Cooldown;
                SetProperty(InternalModel.Cooldown, cooldown, InternalModel, (model, prop) => {
                    model.Cooldown = prop;
                    CostMonitor?.UpdateCost();
                });

                if (cooldown == ECooldownOption.NoneCooldown)
                {
                    UsesCount = 0;
                }
                else
                {
                    if (UsesCount <= 0)
                    {
                        UsesCount = 1;
                    }
                }

                OnPropertyChanged(nameof(NeedToSetUsesCount));
            }
        }

        private ObservableCollection<CooldownOptionVM> _cooldownOptions = [];
        public ObservableCollection<CooldownOptionVM> CooldownOptions
        {
            get => _cooldownOptions;
            set => SetProperty(ref _cooldownOptions, value);
        }

        public int UsesCount
        {
            get => InternalModel.UsesCount;
            set
            {
                SetProperty(InternalModel.UsesCount, value, InternalModel, (model, prop) => {
                    model.UsesCount = prop;
                    CostMonitor?.UpdateCost();
                });
            }
        }

        public bool NeedToSetUsesCount => SelectedCooldownOption?.Cooldown != ECooldownOption.NoneCooldown;
    }

    public class CooldownOptionVM
    {
        public string LocalizedName { get; set; } = string.Empty;

        public ECooldownOption Cooldown { get; set; }

        public override string ToString() => LocalizedName;
    }
}
