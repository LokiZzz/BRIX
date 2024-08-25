using BRIX.Lexica;
using BRIX.Library.Characters;
using BRIX.Library.Items;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Compatibility;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class AddOrEditInventoryItemPageVM(
        ILocalizationResourceManager localization,
        ICharacterService characterService) : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization = localization;
        private readonly ICharacterService _characterService = characterService;

        private EEditingMode _mode;
        private Library.Items.Inventory _inventory = new();
        private Item? _editingItem;

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private InventoryItemVM _item = new(new Item());
        public InventoryItemVM Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private InventoryItemTypeVM? _selectedType;
        public InventoryItemTypeVM? SelectedType
        {
            get => _selectedType;
            set
            {
                if (value != null)
                {
                    UpdateItemType(value);
                }
            }
        }

        private ObservableCollection<InventoryItemTypeVM> _types = [];
        public ObservableCollection<InventoryItemTypeVM> Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
        }

        private InventoryContainerVM? _selectedContainer;
        public InventoryContainerVM? SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                SetProperty(ref _selectedContainer, value);
                if (value != null)
                {
                    MoveItem(value);
                }
            }
        }

        private ObservableCollection<InventoryContainerVM> _containers = [];
        public ObservableCollection<InventoryContainerVM> Containers
        {
            get => _containers;
            set => SetProperty(ref _containers, value);
        }

        private int _oldItemPrice;

        private int _coinsNow;
        public int CoinsNow
        {
            get => _coinsNow;
            set => SetProperty(ref _coinsNow, value);
        }

        public bool ShowCoins => CoinsNow != CoinsWillBe;

        private int _coinsWillBe;
        public int CoinsWillBe
        {
            get => _coinsWillBe;
            set
            {
                if (_inventory != null)
                {
                    _inventory.Coins = value;
                }

                SetProperty(ref _coinsWillBe, value);
                OnPropertyChanged(nameof(ShowCoins));
            }
        }

        private bool _editCoinsAfterSave = true;
        public bool EditCoinsAfterSave
        {
            get => _editCoinsAfterSave;
            set => SetProperty(ref _editCoinsAfterSave, value);
        }

        [RelayCommand]
        public async Task Save()
        {
            if(EditCoinsAfterSave)
            {
                if(CoinsWillBe < 0)
                {
                    await Alert(Localization.InventoryNotEnoughCoinsAlert);

                    return;
                }
            }

            if(_inventory.Items.Where(x => x.Name == Item.Name).Count() > 1 || string.IsNullOrEmpty(Item.Name))
            {
                await Alert(Localization.InventorySameNameAlert);

                return;
            }

            if (!EditCoinsAfterSave)
            {
                _inventory.Coins = CoinsNow;
            }

            Character currentCharacter = await _characterService.GetCurrentCharacterGuaranteed();

            currentCharacter.Inventory = _inventory;
            await _characterService.UpdateAsync(currentCharacter);

            await Navigation.Back(stepsBack: 1, (NavigationParameters.ForceUpdate, true));
        }



        [RelayCommand]
        private async Task AddFeature()
        {
            await Navigation.NavigateAsync<AOEAbilityPage>(
                (NavigationParameters.EditMode, EEditingMode.Add),
                (NavigationParameters.EditAbilityFor, EAbilityFor.Artifact)
            );
        }

        [RelayCommand]
        private async Task EditFeature(ArtifactFeatureModel feature)
        {
            await Navigation.NavigateAsync<AOEAbilityPage>(
                (NavigationParameters.Ability, feature.Copy()),
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.EditAbilityFor, EAbilityFor.Artifact)
            );
        }

        [RelayCommand]
        private async Task RemoveFeature(ArtifactFeatureModel feature)
        {
            AlertPopupResult? result = await Ask(Localization.DeleteAbilityQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                Item?.RemoveFeature(feature);
            }
        }

        [RelayCommand]
        private static async Task ShowDescription(ArtifactFeatureModel feature)
        {
            await Alert(
                new AlertPopupParameters
                {
                    Mode = EAlertMode.ShowMessage,
                    Title = feature.Name,
                    Message = await feature.Internal.ToFullShortLexis()
                }
            );
        }

        private void HandleBackFromEditing(IDictionary<string, object> query)
        {
            ArtifactFeatureModel? editedFeature = 
                query.GetParameterOrDefault<ArtifactFeatureModel>(NavigationParameters.Ability);

            if (editedFeature != null && Item != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch (mode)
                {
                    case EEditingMode.Add:
                        Item.AddFeature(editedFeature);
                        break;
                    case EEditingMode.Edit:
                        Item.UpdateFeature(editedFeature);
                        break;
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            HandleBackFromEditing(query);

            if (_mode == EEditingMode.None)
            {
                _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
                _inventory = query.GetParameterOrDefault<Library.Items.Inventory>(NavigationParameters.Inventory)
                    ?? throw new Exception("В параметры страницы редактирования предмета не передан инвентарь персонажа.");
                Item? originalItem = query.GetParameterOrDefault<Item>(
                    NavigationParameters.InventoryItem
                );

                _editingItem = _inventory.Items.FirstOrDefault(x => x.Name == originalItem?.Name);

                if (_editingItem == null)
                {
                    Item = new InventoryItemVM(new Item());
                    _inventory.Content.Add(Item.InternalModel);
                }
                else
                {
                    Item = new InventoryItemVM(_editingItem);
                }

                CoinsNow = _inventory.Coins;
                CoinsWillBe = _inventory.Coins;
                _oldItemPrice = Item.FullPrice;
                Item.OnFullPriceChanged += OnPriceChanged;

                InitializeItemTypes();
                InitializeContainers();
                InitializeTitle();
            }

            query.Clear();
        }

        private void OnPriceChanged(object? sender, int e)
        {
            int difference = _oldItemPrice - e;
            CoinsWillBe = CoinsNow + difference;
        }

        private void InitializeContainers()
        {
            List<InventoryContainerVM> containers = _inventory.Items
                .Where(x => x is Container && x != Item.InternalModel)
                .Select(x => new InventoryContainerVM { Name = x.Name, OriginalModelRefernece = (Container)x })
                .ToList();
            containers.Add(new InventoryContainerVM { Name = Localization.Inventory });
            Containers = new(containers);
            InventoryContainerVM? selectedContainer = containers.FirstOrDefault(x =>
                x.OriginalModelRefernece != null
                && x.OriginalModelRefernece?.Payload?.Contains(Item.InternalModel) == true
            );

            SelectedContainer = selectedContainer ?? Containers.First(x => x.OriginalModelRefernece == null);
        }

        private void InitializeItemTypes()
        {
            Types = new ObservableCollection<InventoryItemTypeVM>(
                Enum.GetValues<EInventoryItemType>().Select(x => new InventoryItemTypeVM {
                    Text = _localization[x.ToString("G")].ToString() ?? string.Empty,
                    Type = x
                })
            );

            SelectedType = Types.Single(x => x.Type == Item.Type);
        }

        private void InitializeTitle()
        {
            switch(_mode)
            {
                case EEditingMode.Add:
                    Title = Localization.AddInventoryItem; break;
                case EEditingMode.Edit:
                    Title = Localization.EditInventoryItem; break;
            }
        }

        private async void UpdateItemType(InventoryItemTypeVM itemType)
        {
            if (SelectedType == null)
            {
                SetProperty(ref _selectedType, itemType, nameof(SelectedType));
            }

            if (SelectedType?.Type == itemType?.Type || itemType == null)
            {
                return;
            }

            if(_mode == EEditingMode.Add)
            {
                Item.Type = itemType.Type;
                SetProperty(ref _selectedType, itemType, nameof(SelectedType));
                ReplaceItemWithNewType(itemType);

                return;
            }

            if(SelectedType == null)
            {
                throw new Exception("Inconsistent item type was set.");
            }

            bool wasContainerAndNowIsNot = SelectedType.Type == EInventoryItemType.Container
                && itemType.Type != EInventoryItemType.Container
                && (Item.InternalModel as Container)?.Payload?.Count > 0;

            if (wasContainerAndNowIsNot)
            {
                AlertPopupResult? result = await Ask(Localization.InventoryItemWasConatinerAlert);

                if (result?.Answer == EAlertPopupResult.No)
                {
                    if (Item.InternalModel is Container container)
                    {
                        _inventory.MoveContentUpper(container);
                    }
                }
            }

            ReplaceItemWithNewType(itemType);
            OnPropertyChanged(nameof(ShowCoins));
        }

        private void ReplaceItemWithNewType(InventoryItemTypeVM value)
        {
            Item.Type = value.Type;
            SetProperty(ref _selectedType, value, nameof(SelectedType));
            Item? existingItem = _inventory.Items.FirstOrDefault(x => x.Equals(Item.InternalModel));

            if (existingItem != null)
            {
                _inventory.Swap(existingItem, Item.InternalModel);
            }
        }

        private void MoveItem(InventoryContainerVM value)
        {
            if(value == null)
            {
                return;
            }

            Container? oldContainer = _inventory.Items.FirstOrDefault(x =>
                x is Container container && container.Payload.Contains(Item.InternalModel)
            ) as Container;
            Container? newContainer = value.OriginalModelRefernece;

            if(oldContainer == newContainer)
            {
                return;
            }
            else
            {
                if(oldContainer == null)
                {
                    _inventory.Content.Remove(Item.InternalModel);
                }
                else
                {
                    oldContainer.Payload.Remove(Item.InternalModel);
                }

                if (newContainer == null)
                {
                    _inventory.Content.Add(Item.InternalModel);
                }
                else
                {
                    newContainer.Payload.Add(Item.InternalModel);
                }
            }
        }
    }
}