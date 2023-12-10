using BRIX.Library;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AOEStatusPageVM : ViewModelBase, IQueryAttributable
    {
        public AOEStatusPageVM(ICharacterService characterService)
        {
            CharacterService = characterService;
        }

        private Character? _currentCharacrter;

        public ICharacterService CharacterService { get; }

        EEditingMode _mode;

        private string _title = string.Empty;
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

        private StatusItemVM _status = new(new());
        public StatusItemVM Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        [RelayCommand]
        public async Task Save()
        {
            await Navigation.Back(
                stepsBack: 1,
                (NavigationParameters.EditMode, _mode),
                (NavigationParameters.Status, Status)
            );
        }

        [RelayCommand]
        public async Task AddEffect()
        {
            await Navigation.NavigateAsync<ChooseEffectPage>(
                (NavigationParameters.CostMonitor, new AbilityCostMonitorPanelVM()),
                (NavigationParameters.ForStatus, true)
            );
        }

        [RelayCommand]
        public async Task EditEffect(EffectModelBase effect)
        {
            await Navigation.NavigateAsync(
                EffectsDictionary.GetEditPageRoute(effect),
                ENavigationMode.Push,
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.Effect, effect.Copy()),
                (NavigationParameters.CostMonitor, new AbilityCostMonitorPanelVM())
            );
        }

        [RelayCommand]
        public async Task DeleteEffect(EffectModelBase effect)
        {
            AlertPopupResult? result = await Ask(Localization.DeleteEffectQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                Status.RemoveEffect(effect);
            }
        }

        [RelayCommand]
        public async Task FromAbility()
        {
            if(_currentCharacrter == null)
            {
                return;
            }

            PickerPopupResult? result =
                await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(
                new PickerPopupParameters
                {
                    Title = Localization.Abilities,
                    SelectMultiple = false,
                    Items = _currentCharacrter.StatusAbilities.Cast<object>().ToList(),
                }
            );

            if(result != null && result?.SelectedItem != null)
            {
                Library.Ability.Status status = ((CharacterAbility)result.SelectedItem).BuildStatus();
                Status = new StatusItemVM(status);
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (_mode == EEditingMode.None)
            {
                _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
                Status = query.GetParameterOrDefault<StatusItemVM>(NavigationParameters.Status)
                    ?? new StatusItemVM(new Library.Ability.Status());
                InitializeTitle();
                _currentCharacrter = await CharacterService.GetCurrentCharacterGuaranteed();
            }
            else
            {
                await HandleBackFromEditing(query);
            }

            query?.Clear();
        }

        private Task HandleBackFromEditing(IDictionary<string, object> query)
        {
            EffectModelBase? editedEffect = query.GetParameterOrDefault<EffectModelBase>(NavigationParameters.Effect);

            if (editedEffect != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch (mode)
                {
                    case EEditingMode.Add:
                        Status.AddEffect(editedEffect);
                        break;
                    case EEditingMode.Edit:
                        Status.UpdateEffect(editedEffect);
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private void InitializeTitle()
        {
            switch(_mode)
            {
                case EEditingMode.Add:
                    Title = Localization.AddStatus;
                    break;
                case EEditingMode.Edit:
                    Title = Localization.EditStatus;
                    break;
            }
        }
    }
}
