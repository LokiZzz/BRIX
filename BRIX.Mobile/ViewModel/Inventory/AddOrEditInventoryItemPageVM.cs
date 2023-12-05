using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class AddOrEditInventoryItemPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;
        private readonly ICharacterService _characterService;

        private EEditingMode _mode;
        private Library.Characters.Inventory _inventory = new();
        private InventoryItem? _editingItem;

        public AddOrEditInventoryItemPageVM(
            ILocalizationResourceManager localization, 
            ICharacterService characterService)
        {
            _localization = localization;
            _characterService = characterService;
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private InventoryItemVM _item = new InventoryItemVM(new InventoryItem());
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

        private ObservableCollection<InventoryItemTypeVM> _types = new();
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

        private ObservableCollection<InventoryContainerVM> _containers = new();
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

            if(_inventory.Items.Where(x => x.Name == Item.Name).Count() > 1)
            {
                await Alert(Localization.InventorySameNameAlert);

                return;
            }

            if (!EditCoinsAfterSave)
            {
                _inventory.Coins = CoinsNow;
            }

            Character currentCharacter = await _characterService.GetCurrentCharacterGuaranteed();
            bool affectsAbility = _mode == EEditingMode.Edit 
                && Item.InternalModel is MaterialSupport material
                && currentCharacter.HaveMaterialDependedAbilities(material) == true;

            if (affectsAbility)
            {
                if (Item.InternalModel is MaterialSupport materialSupport)
                {
                    if (currentCharacter.UpdateMaterialSupport(materialSupport) == false)
                    {
                        await Alert(Localization.InventoryNotEnoughEXPForChanges);

                        return;
                    }
                }
            }

            currentCharacter.Inventory = _inventory;
            await _characterService.UpdateAsync(currentCharacter);

            await Navigation.Back(stepsBack: 1, (NavigationParameters.ForceUpdate, true));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            _inventory = query.GetParameterOrDefault<Library.Characters.Inventory>(NavigationParameters.Inventory);
            InventoryItem originalItem = query.GetParameterOrDefault<InventoryItem>(
                NavigationParameters.InventoryItem
            );

            _editingItem = _inventory.Items.FirstOrDefault(x => x.Name == originalItem?.Name);

            if(_editingItem == null)
            {
                Item = new InventoryItemVM(new InventoryItem());
                _inventory.Content.Add(Item.InternalModel);
            }
            else
            {
                Item = new InventoryItemVM(_editingItem);
            }

            CoinsNow = _inventory.Coins; ;
            CoinsWillBe = _inventory.Coins;
            _oldItemPrice = Item.FullPrice;
            Item.OnFullPriceChanged += OnPriceChanged;

            InitializeItemTypes();
            InitializeContainers();
            InitializeTitle();

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

            if (SelectedType.Type.IsMaterial() && !itemType.Type.IsMaterial())
            { 
                Character character = await _characterService.GetCurrentCharacterGuaranteed();

                if (Item.InternalModel is MaterialSupport materialSupport)
                {
                    bool canRemove = character.CanRemoveMaterialSupport(materialSupport);

                    if (!canRemove)
                    {
                        await Alert(Localization.InventoryNotEnoughEXPForChanges);
                        OnPropertyChanged(nameof(SelectedType));

                        return;
                    }
                }
            }

            bool wasContainerAndNowIsNot = SelectedType.Type == EInventoryItemType.Container
                && itemType.Type != EInventoryItemType.Container
                && (Item.InternalModel as Container)?.Payload?.Any() == true;

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
            InventoryItem? existingItem = _inventory.Items.FirstOrDefault(x => x.Equals(Item.InternalModel));

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

    public class InventoryItemTypeVM
    {
        public EInventoryItemType Type { get; set; }
        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }

    public class InventoryContainerVM
    {
        public Container? OriginalModelRefernece { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;
    }
}