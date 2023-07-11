using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Inventory;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Inventory;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterInventoryPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;
        private Character _currentCharacter;
        private InventoryItemConverter _vmConverter;

        public CharacterInventoryPageVM(ICharacterService characterService)
        {
            WeakReferenceMessenger.Default.Register<CurrentCharacterChanged>(
                this,
                async (r, m) => await Initialize(true)
            );
            _characterService = characterService;
            _vmConverter = new InventoryItemConverter();
        }

        private ObservableCollection<InventoryItemNodeVM> _inventoryItems;
        public ObservableCollection<InventoryItemNodeVM> InventoryItems
        {
            get => _inventoryItems;
            set => SetProperty(ref _inventoryItems, value);
        }

        private int _coins;
        public int Coins
        {
            get => _coins;
            set => SetProperty(ref _coins, value);
        }

        public override async Task OnNavigatedAsync()
        {
            await Initialize(false);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool update = query.GetParameterOrDefault<bool>(NavigationParameters.ForceUpdate);

            if(update)
            {
                await Initialize(force: true);
            }
        }

        private async Task Initialize(bool force = false) 
        {
            if(InventoryItems == null || force)
            {
                _currentCharacter = await _characterService.GetCurrentCharacter();

                InventoryItems = new(
                    _currentCharacter.Inventory.Content.Select(_vmConverter.ToVM).ToList()
                );

                Coins = _currentCharacter.Inventory.Coins;
            }
        }

        [RelayCommand]
        public async Task ShowDescription(InventoryItemNodeVM item)
        {
            await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                new AlertPopupParameters 
                {
                    Title = item.Name,
                    Message = string.IsNullOrEmpty(item.Description) ? $"{item.Name}..." : item.Description,
                }
            );
        }

        [RelayCommand]
        public async Task Delete(InventoryItemNodeVM item)
        {
            if (item.InternalModel is MaterialSupport material && !_currentCharacter.CanRemoveMaterialSupport(material))
            {
                await Alert(Localization.InventoryNotEnoughEXPForDelete);

                return;
            }

            AlertPopupResult result = await Ask(string.Format(Localization.AskToDeleteInventoryItem, item.Name));

            if(result?.Answer == EAlertPopupResult.No)
            {
                return;
            }

            bool saveContent = false;

            if(item.Type == EInventoryItemType.Container && item.Payload.Any())
            {
                AlertPopupResult resultDeleteContent = await Ask(
                    string.Format(Localization.AskDeleteContainerWithContent, item.Name)
                );

                saveContent = resultDeleteContent?.Answer == EAlertPopupResult.No;
            }

            bool changesAffectsAbilities = item.InternalModel is MaterialSupport materialToRemove
                && _currentCharacter.HaveMaterialDependedAbilities(materialToRemove);

            if (changesAffectsAbilities)
            {
                AlertPopupResult willRisePriceResult = await Ask(Localization.InventoryAbilitiesWillRisePrice);

                if(willRisePriceResult?.Answer == EAlertPopupResult.No)
                {
                    return;
                }

                _currentCharacter.RemoveMaterialSupport(item.InternalModel as MaterialSupport, saveContent);
            }
            else
            {
                _currentCharacter.Inventory.Remove(item.InternalModel, saveContent);
            }

            await _characterService.UpdateAsync(_currentCharacter);
            await Initialize(force: true);
        }

        [RelayCommand]
        public async Task Edit(InventoryItemNodeVM item)
        {
            await Navigation.NavigateAsync<AddOrEditInventoryItemPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.Inventory, _currentCharacter.Inventory.Copy()),
                (NavigationParameters.InventoryItem, item.InternalModel)
            );
        }

        [RelayCommand]
        public async Task New()
        {
            await Navigation.NavigateAsync<AddOrEditInventoryItemPage>(
                (NavigationParameters.EditMode, EEditingMode.Add),
                (NavigationParameters.Inventory, _currentCharacter.Inventory.Copy())
            );
        }

        [RelayCommand]
        public async Task EditCoins()
        {
            NumericEditorResult result = 
                await ShowPopupAsync<NumericEditorPopup, NumericEditorResult, NumericEditorParameters>(
                    new NumericEditorParameters { Title = Localization.Coins }
            );

            if(result != null)
            {
                int newValue = result.ToValue(_currentCharacter.Inventory.Coins);
                newValue = newValue < 0 ? 0 : newValue;
                
                _currentCharacter.Inventory.Coins = newValue;
                Coins = newValue;

                await _characterService.UpdateAsync(_currentCharacter);
            }
        }

        [RelayCommand]
        public async Task AdjustCount(InventoryItemVM itemToEdit)
        {
            NumericEditorResult result =
                await ShowPopupAsync<NumericEditorPopup, NumericEditorResult, NumericEditorParameters>(
                    new NumericEditorParameters { Title = itemToEdit.Name }
            );

            if (result != null)
            {
                int newValue = result.ToValue(itemToEdit.Count);
                newValue = newValue < 0 ? 0 : newValue;
                int oldValue = itemToEdit.Count;

                itemToEdit.Count = newValue;

                if(itemToEdit.Type == EInventoryItemType.Consumable)
                {
                    AlertPopupResult askAdjustCoinsReuslt = await Ask(Localization.InventoryAskAdjustCoinsAlert);

                    if(askAdjustCoinsReuslt?.Answer == EAlertPopupResult.Yes)
                    {
                        int coinsDiff = (newValue - oldValue) * itemToEdit.Price;
                        int newCoinsValue = _currentCharacter.Inventory.Coins - coinsDiff;
                        _currentCharacter.Inventory.Coins = newCoinsValue >= 0 ? newCoinsValue : 0;
                        Coins = _currentCharacter.Inventory.Coins;
                    }
                }

                await _characterService.UpdateAsync(_currentCharacter);
            }
        }
    }
}
