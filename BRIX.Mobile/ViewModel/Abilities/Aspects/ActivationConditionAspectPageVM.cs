using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Enums;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ActivationConditionsAspectPageVM(ILocalizationResourceManager localization) 
        : AspectPageVMBase<ActivationConditionsAspectModel>
    {
        private readonly ILocalizationResourceManager Localization = localization;
        private ObservableCollection<ActivationConditionOptionVM> _conditions = [];
        public ObservableCollection<ActivationConditionOptionVM> Conditions
        {
            get => _conditions;
            set => SetProperty(ref _conditions, value);
        }

        public bool ShowNoConditionsText => Conditions?.Any() == false;

        [RelayCommand]
        public async Task AddCondition()
        {
            List<EActivationCondition> customConditions =
            [
                EActivationCondition.EasyActivationCondition,
                EActivationCondition.MediumActivationCondition,
                EActivationCondition.HardActivationCondition
            ];

            List<object> allConditions = Enum.GetValues<EActivationCondition>()
                .Select(x => new ActivationConditionOptionVM
                {
                    Condition = x,
                    Text = Localization[x.ToString("G")].ToString() ?? string.Empty
                })
                .Where(x => !Conditions.Any(y => y.Condition == x.Condition)
                    || customConditions.Any(y => y == x.Condition))
                .Select(x => x as object)
                .ToList();

            PickerPopupResult? result = await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(
                new()
                {
                    Title = Resources.Localizations.Localization.ActivationCondition,
                    Items = allConditions,
                }
            );

            if (result?.SelectedItem != null)
            {
                ActivationConditionOptionVM concreteResult = (ActivationConditionOptionVM)result.SelectedItem;

                if (customConditions.Any(x => x == concreteResult.Condition))
                {
                    string message = ActivationConditionsAspectPageVM.GetCustomConditionHint(concreteResult.Condition);

                    EntryPopupResult? entryResult = await ShowPopupAsync<EntryPopup, EntryPopupResult, EntryPopupParameters>(
                        new EntryPopupParameters
                        {
                            Title = Resources.Localizations.Localization.ActivationCondition,
                            Message = message,
                            Placeholder = Resources.Localizations.Localization.ActivationCondition,
                            ButtonText = Resources.Localizations.Localization.Ok,
                        }
                    );

                    if (entryResult == null)
                    {
                        return;
                    }

                    concreteResult.Text = entryResult.Text;
                }

                if (Conditions.Any(x => x.Condition == concreteResult.Condition && x.Text == concreteResult.Text))
                {
                    await Alert(Resources.Localizations.Localization.ActivationConditionWarning);

                    return;
                }

                Conditions.Add(concreteResult);

                if (Aspect != null)
                {
                    string customCondition = customConditions.Any(x => x == concreteResult.Condition)
                        ? concreteResult.Text
                        : string.Empty;
                    Aspect.Internal.Conditions.Add((concreteResult.Condition, customCondition));
                }

                OnPropertyChanged(nameof(ShowNoConditionsText));
            }

            CostMonitor?.UpdateCost();
        }

        [RelayCommand]
        public void DeleteCondition(ActivationConditionOptionVM condition)
        {
            if(Aspect == null)
            {
                return;
            }

            Conditions.Remove(condition);

            (EActivationCondition Type, string Comment) conditionToDelete =
                Aspect.Internal.Conditions.FirstOrDefault(x =>
                    x.Type == condition.Condition || x.Comment == condition.Text);
            Aspect.Internal.Conditions.Remove(conditionToDelete);

            CostMonitor?.UpdateCost();
            OnPropertyChanged(nameof(ShowNoConditionsText));
        }

        private ActivationConditionOptionVM ToConditionVM((EActivationCondition Type, string Comment) condition)
        {
            ActivationConditionOptionVM restrictionVM = new()
            {
                Condition = condition.Type,
                Text = condition.Type switch
                {
                    EActivationCondition.EasyActivationCondition
                    or EActivationCondition.MediumActivationCondition
                    or EActivationCondition.HardActivationCondition
                        => condition.Comment,
                    _ => Localization[condition.Type.ToString("G")].ToString() ?? string.Empty,
                }
            };
            return restrictionVM;
        }

        private static string GetCustomConditionHint(EActivationCondition condition)
        {
            return condition switch
            {
                EActivationCondition.EasyActivationCondition => 
                    Resources.Localizations.Localization.EnterLowActivationCondition,
                EActivationCondition.MediumActivationCondition => 
                    Resources.Localizations.Localization.EnterMediumActiovationCondition,
                EActivationCondition.HardActivationCondition => 
                    Resources.Localizations.Localization.EnterHardActiovationCondition,
                _ => string.Empty,
            };
        }

        public override void Initialize()
        {
            if (Aspect == null)
            {
                return;
            }

            Conditions = new(Aspect.Internal.Conditions.Select(ToConditionVM));
            OnPropertyChanged(nameof(ShowNoConditionsText));
        }
    }
}
