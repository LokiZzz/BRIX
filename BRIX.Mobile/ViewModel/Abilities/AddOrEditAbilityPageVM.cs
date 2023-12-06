using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Abilities;
using BRIX.Utility.Extensions;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using BRIX.Mobile.Models.Characters;
using System.Collections.ObjectModel;
using BRIX.Mobile.ViewModel.Inventory;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AOEAbilityPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;
        private readonly ICharacterService _characterService;
        private readonly InventoryItemConverter _inventoryConverter;
        public AOEAbilityPageVM(ILocalizationResourceManager localization, ICharacterService characterService)
        {
            _localization = localization;
            _characterService = characterService;
            _inventoryConverter = new InventoryItemConverter();
            SaveCommand = new AsyncRelayCommand(Save);
        }

        private Character? _characterCopy;

        private CharacterAbilityModel _ability = new();
        public CharacterAbilityModel Ability
        {
            get => _ability;
            set => SetProperty(ref _ability, value);
        }


        private AbilityCostMonitorPanelVM _costMonitor = new();
        public AbilityCostMonitorPanelVM CostMonitor
        {
            get => _costMonitor;
            set => SetProperty(ref _costMonitor, value);
        }

        private EEditingMode _mode;
        public EEditingMode Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<InventoryItemNodeVM> _availiableMaterialSupport = [];
        public ObservableCollection<InventoryItemNodeVM> AvailiableMaterialSupport
        {
            get => _availiableMaterialSupport;
            set => SetProperty(ref _availiableMaterialSupport, value);
        }

        private ObservableCollection<InventoryItemNodeVM> _materialSupport = [];
        public ObservableCollection<InventoryItemNodeVM> MaterialSupport
        {
            get => _materialSupport;
            set => SetProperty(ref _materialSupport, value);
        }

        private IAsyncRelayCommand? _saveCommand;
        public IAsyncRelayCommand? SaveCommand
        {
            get => _saveCommand;
            set => SetProperty(ref _saveCommand, value);
        }

        public async Task Save()
        {
            if(_characterCopy == null)
            {
                return;
            }

            if(CostMonitor.EXPOverflow)
            {
                await Alert(Localization.AbilityEXPOverflowMessage);

                return;
            }

            await Navigation.Back(
                stepsBack: 1,
                (NavigationParameters.EditMode, Mode), 
                (NavigationParameters.Ability, Ability),
                (NavigationParameters.MaterialSupport, _characterCopy.MaterialSupport)
            );
        }

        [RelayCommand]
        public async Task AddEffect()
        {
            await Navigation.NavigateAsync<ChooseEffectPage>(
                (NavigationParameters.CostMonitor, CostMonitor.Copy())
            );
        }

        [RelayCommand]
        public async Task EditEffect(EffectModelBase effectToEdit)
        {
            await Navigation.NavigateAsync(
                EffectsDictionary.GetEditPageRoute(effectToEdit),
                ENavigationMode.Push,
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.Effect, effectToEdit.Copy()),
                (NavigationParameters.CostMonitor, CostMonitor.Copy())
            );
        }

        [RelayCommand]
        public async Task DeleteEffect(EffectModelBase effectToRemove)
        {
            AlertPopupResult? result = await Ask(Localization.DeleteEffectQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                Ability.RemoveEffect(effectToRemove);
            }
        }

        [RelayCommand]
        public async Task AddMaterial()
        {
            if (_characterCopy == null)
            {
                return;
            }

            IEnumerable<InventoryItem> availiableItems = _characterCopy.Inventory.Items.Where(x =>
                !MaterialSupport.Any(y => y.Name == x.Name) && (x is Equipment || x is Consumable)
            );
            IEnumerable<InventoryItemNodeVM> availiableItemsNodes = availiableItems.Select(_inventoryConverter.ToVM);

            PickerPopupResult? result = 
                await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(
                new PickerPopupParameters
                {
                    Title = Localization.MaterialSupport,
                    SelectMultiple = true,
                    Items = availiableItemsNodes.Cast<object>().ToList(),
                }
            );

            if (result != null && result.SelectedItems.Any())
            {
                IEnumerable<InventoryItemNodeVM> itemNodes = result.SelectedItems.Cast<InventoryItemNodeVM>();

                foreach (InventoryItemNodeVM item in itemNodes)
                {
                    MaterialSupport.Add(item);
                    _characterCopy.MaterialSupport.Add(new AbilityMaterialSupport { 
                        AbilityId = Ability.InternalModel.Id,
                        MaterialSupportId = item.InternalModel.Id
                    });
                }

                CostMonitor.UpdateCost();
            }
        }

        [RelayCommand]
        public async Task DeleteMaterial(InventoryItemNodeVM itemToRemove)
        {
            if (_characterCopy == null)
            {
                return;
            }

            AlertPopupResult? result = await Ask(
                string.Format(Localization.AskDeleteMaterialSupport, itemToRemove.Name)
            );

            if(result?.Answer != EAlertPopupResult.Yes)
            {
                return;
            }

            MaterialSupport.Remove(itemToRemove);
            _characterCopy.MaterialSupport.RemoveAll(x => 
                x.AbilityId == Ability.InternalModel.Id
                && x.MaterialSupportId == itemToRemove.InternalModel.Id
            );

            CostMonitor.UpdateCost();
        }

        public override Task OnNavigatedAsync()
        {
            switch (Mode)
            {
                case EEditingMode.Add:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Add].ToString() ?? string.Empty;
                    break;
                case EEditingMode.Edit:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Edit].ToString() ?? string.Empty;
                    break;
            }

            return Task.CompletedTask;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (Mode == EEditingMode.None)
            {
                _characterCopy = (await _characterService.GetCurrentCharacter()).Copy();
                Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
                Ability = query.GetParameterOrDefault<CharacterAbilityModel>(NavigationParameters.Ability)
                    ?? new CharacterAbilityModel(new CharacterAbility());
                Ability.Character = _characterCopy;
                IntitializeCostMonitor();
                IntitializeMaterialSupport();
            }
            else
            {
                await HandleBackFromEditing(query);
            }

            CostMonitor.SaveCommand = SaveCommand 
                ?? throw new Exception("Команда сохранения для CostMonitor не инициализирована.");

            query.Clear();
        }

        private Task HandleBackFromEditing(IDictionary<string, object> query)
        {
            EffectModelBase editedEffect = query.GetParameterOrDefault<EffectModelBase>(NavigationParameters.Effect);

            if(editedEffect != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch (mode)
                {
                    case EEditingMode.Add:
                        Ability.AddEffect(editedEffect);
                        break;
                    case EEditingMode.Edit:
                        Ability.UpdateEffect(editedEffect);
                        break;
                }
            }

            CostMonitor.UpdateCost();

            return Task.CompletedTask;
        }

        private void IntitializeMaterialSupport()
        {
            if (_characterCopy == null)
            {
                return;
            }

            List<InventoryItemNodeVM> materials = _characterCopy.GetMaterialSupportForAbility(Ability.InternalModel)
                .Select(_inventoryConverter.ToVM)
                .ToList();

            MaterialSupport = new(materials);
        }

        private void IntitializeCostMonitor()
        {
            CostMonitor = new AbilityCostMonitorPanelVM(
                Ability, 
                SaveCommand ?? throw new Exception("Команда сохранения для CostMonitor не инициализирована.")
            );
        }
    }
}
