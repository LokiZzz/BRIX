﻿using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Enums;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ActivationConditionsAspectPageVM : AspectPageVMBase<ActivationConditionsAspectModel>
    {
        private readonly ILocalizationResourceManager Localization;

        public ActivationConditionsAspectPageVM(ILocalizationResourceManager localization)
        {
            Localization = localization;
        }

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
                    string message = GetCustomConditionHint(concreteResult.Condition);

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
                    Aspect.Internal.Conditions.Add((concreteResult.Condition, concreteResult.Text));
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

            (Enum Type, string Comment) conditionToDelete =
                Aspect.Internal.Conditions.FirstOrDefault(x =>
                    x.Type == (Enum)condition.Condition || x.Comment == condition.Text);
            Aspect.Internal.Conditions.Remove(conditionToDelete);

            CostMonitor?.UpdateCost();
            OnPropertyChanged(nameof(ShowNoConditionsText));
        }

        private ActivationConditionOptionVM ToConditionVM((Enum Type, string Comment) condition)
        {
            ActivationConditionOptionVM restrictionVM = new() { Condition = (EActivationCondition)condition.Type };

            switch (condition.Type)
            {
                case EActivationCondition.EasyActivationCondition:
                case EActivationCondition.MediumActivationCondition:
                case EActivationCondition.HardActivationCondition:
                    restrictionVM.Text = condition.Comment;
                    break;
                default:
                    restrictionVM.Text = Localization[condition.Type.ToString("G")].ToString() ?? string.Empty;
                    break;
            }

            return restrictionVM;
        }

        private string GetCustomConditionHint(EActivationCondition condition)
        {
            switch (condition)
            {
                case EActivationCondition.EasyActivationCondition:
                    return Resources.Localizations.Localization.EnterLowActivationCondition;
                case EActivationCondition.MediumActivationCondition:
                    return Resources.Localizations.Localization.EnterMediumActiovationCondition;
                case EActivationCondition.HardActivationCondition:
                    return Resources.Localizations.Localization.EnterHardActiovationCondition;
                default:
                    return string.Empty;
            }
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
