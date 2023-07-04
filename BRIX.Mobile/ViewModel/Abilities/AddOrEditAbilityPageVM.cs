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
    public partial class AddOrEditAbilityPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;
        private readonly ICharacterService _characterService;
        private readonly InventoryItemConverter _inventoryConverter;
        public AddOrEditAbilityPageVM(ILocalizationResourceManager localization, ICharacterService characterService)
        {
            _localization = localization;
            _characterService = characterService;
            _inventoryConverter = new InventoryItemConverter();
        }

        private AbilityModel _ability;
        public AbilityModel Ability
        {
            get => _ability;
            set => SetProperty(ref _ability, value);
        }


        private AbilityCostMonitorPanelVM _costMonitor;
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

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private ObservableCollection<InventoryItemNodeVM> _availiableMaterialSupport;
        public ObservableCollection<InventoryItemNodeVM> AvailiableMaterialSupport
        {
            get => _availiableMaterialSupport;
            set => SetProperty(ref _availiableMaterialSupport, value);
        }

        private ObservableCollection<InventoryItemNodeVM> _materialSupport;
        public ObservableCollection<InventoryItemNodeVM> MaterialSupport
        {
            get => _materialSupport;
            set => SetProperty(ref _materialSupport, value);
        }

        [RelayCommand]
        public async Task Save()
        {
            if(CostMonitor.EXPOverflow)
            {
                await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(new AlertPopupParameters { 
                    Mode = EAlertMode.ShowMessage,
                    Title = Localization.NotEnoughEXP,
                    OkText = Localization.Ok,
                    Message = Localization.AbilityEXPOverflowMessage
                });

                return;
            }

            await Navigation.Back(
                stepsBack: 1,
                (NavigationParameters.EditMode, Mode), 
                (NavigationParameters.Ability, Ability)
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
            AlertPopupResult result = await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                new AlertPopupParameters
                {
                    Title = _localization[LocalizationKeys.Warning].ToString(),
                    Message = _localization[LocalizationKeys.DeleteEffectQuestion].ToString(),
                    YesText = _localization[LocalizationKeys.Yes].ToString(),
                    NoText = _localization[LocalizationKeys.No].ToString()
                }
            );

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                Ability.RemoveEffect(effectToRemove);
            }
            
        }

        [RelayCommand]
        public async Task AddMaterial()
        {

        }

        [RelayCommand]
        public async Task DeleteMaterial(InventoryItemNodeVM itemToRemovove)
        {
            
        }

        public override Task OnNavigatedAsync()
        {
            switch (Mode)
            {
                case EEditingMode.Add:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Add].ToString();
                    break;
                case EEditingMode.Edit:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Edit].ToString();
                    break;
            }

            return Task.CompletedTask;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (Mode == EEditingMode.None)
            {
                Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
                Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability)
                    ?? new AbilityModel(new Ability());
                await IntitializeCostMonitor();
                IntitializeMaterialSupport();
                await InitializeAvailiableMaterialSupport();
            }
            else
            {
                await HandleBackFromEditing(query);
            }

            CostMonitor.SaveCommand = SaveCommand;

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
            List<InventoryItemNodeVM> equipment = Ability.InternalModel.Equipment
                .Select(_inventoryConverter.ToVM)
                .ToList();
            List<InventoryItemNodeVM> consumables = Ability.InternalModel.Consumables
                .Select(_inventoryConverter.ToVM)
                .ToList();

            MaterialSupport = new(equipment.Union(consumables));
        }

        private async Task InitializeAvailiableMaterialSupport()
        {
            Character currentCharacter = await _characterService.GetCurrentCharacter();
            IEnumerable<InventoryItem> availiableItems = currentCharacter.Inventory.Items.Where(x =>
                !MaterialSupport.Any(y => y.Name == x.Name) && (x is Equipment || x is Consumable)
            );
            IEnumerable<InventoryItemNodeVM> availiableItemsNodes = availiableItems.Select(_inventoryConverter.ToVM);

            AvailiableMaterialSupport = new(availiableItemsNodes);
        }

        private async Task IntitializeCostMonitor()
        {
            Character currentCharacter = await _characterService.GetCurrentCharacter();
            CostMonitor = new AbilityCostMonitorPanelVM(
                Ability, 
                SaveCommand, 
                new CharacterModel(currentCharacter)
            );
        }
    }
}
