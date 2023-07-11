using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Inventory
{
    public partial class AddOrEditInventoryItemPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;

        private EEditingMode _mode;
        private Library.Characters.Inventory _inventory;
        private InventoryItem _editingItem;

        public AddOrEditInventoryItemPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private InventoryItemVM _item;
        public InventoryItemVM Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private InventoryItemTypeVM _selectedType;
        public InventoryItemTypeVM SelectedType
        {
            get => _selectedType;
            set => UpdateItemType(value);
        }

        private ObservableCollection<InventoryItemTypeVM> _types;
        public ObservableCollection<InventoryItemTypeVM> Types
        {
            get => _types;
            set => SetProperty(ref _types, value);
        }

        private InventoryContainerVM _selectedContainer;
        public InventoryContainerVM SelectedContainer
        {
            get => _selectedContainer;
            set
            {
                SetProperty(ref _selectedContainer, value);
                MoveItem(value);
            }
        }

        private ObservableCollection<InventoryContainerVM> _containers;
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
            //&& (SelectedType?.Type == EInventoryItemType.Equipment 
            //    || SelectedType?.Type == EInventoryItemType.Consumable);

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
                    await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>( 
                        new AlertPopupParameters 
                        {
                            Message = Localization.InventoryNotEnoughCoinsAlert
                        }
                    );

                    return;
                }
            }

            if(_inventory.Items.Where(x => x.Name == Item.Name).Count() > 1)
            {
                await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                    new AlertPopupParameters
                    {
                        Message = Localization.InventorySameNameAlert
                    }
                );

                return;
            }

            if (!EditCoinsAfterSave)
            {
                _inventory.Coins = CoinsNow;
            }

            await Navigation.Back(stepsBack: 1, 
                (NavigationParameters.Inventory, _inventory)
            );
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

        private void OnPriceChanged(object sender, int e)
        {
            int difference = _oldItemPrice - e;
            CoinsWillBe = CoinsNow + difference;
        }

        private void InitializeContainers()
        {
            List<InventoryContainerVM> containers = _inventory.Items
                .Where(x => x is Container && x != Item.InternalModel)
                .Select(x => new InventoryContainerVM { Name = x.Name, OriginalModelRefernece = x as Container })
                .ToList();
            containers.Add(new InventoryContainerVM { Name = Localization.Inventory });
            Containers = new(containers);
            InventoryContainerVM selectedContainer = containers.FirstOrDefault(x =>
                x.OriginalModelRefernece != null
                && x.OriginalModelRefernece?.Payload?.Contains(Item.InternalModel) == true
            );

            SelectedContainer = selectedContainer ?? Containers.First(x => x.OriginalModelRefernece == null);
        }

        private void InitializeItemTypes()
        {
            Types = new ObservableCollection<InventoryItemTypeVM>(
                Enum.GetValues<EInventoryItemType>().Select(x => new InventoryItemTypeVM {
                    Text = _localization[x.ToString("G")].ToString(),
                    Type = x
                })
            );

            SelectedType = Types.FirstOrDefault(x => x.Type == Item.Type);
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

        private async void UpdateItemType(InventoryItemTypeVM value)
        {
            if (SelectedType == null)
            {
                SetProperty(ref _selectedType, value, nameof(SelectedType));
            }

            if (SelectedType?.Type == value?.Type)
            {
                return;
            }

            bool wasContainerAndNowIsNot = SelectedType.Type == EInventoryItemType.Container
                && value.Type != EInventoryItemType.Container
                && (Item.InternalModel as Container)?.Payload?.Any() == true;

            if (wasContainerAndNowIsNot)
            {
                AlertPopupResult result = await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                    new AlertPopupParameters
                    {
                        Mode = EAlertMode.AskYesOrNo,
                        Message = Localization.InventoryItemWasConatinerAlert,
                    }
                );

                if (result?.Answer == EAlertPopupResult.No)
                {
                    _inventory.MoveContentUpper(Item.InternalModel as Container);
                }
            }

            ReplaceItemWithNewType(value);
            OnPropertyChanged(nameof(ShowCoins));
        }

        private void ReplaceItemWithNewType(InventoryItemTypeVM value)
        {
            Container container = _inventory.Items.Where(x =>
                            x is Container container && container.Payload.Contains(Item.InternalModel)
                        ).FirstOrDefault() as Container;
            int index = container != null
                ? container.Payload.IndexOf(Item.InternalModel)
                : _inventory.Content.IndexOf(Item.InternalModel);

            _inventory.Remove(Item.InternalModel);
            Item.Type = value.Type;
            SetProperty(ref _selectedType, value, nameof(SelectedType));

            if (container == null)
            {
                _inventory.Content.Insert(index, Item.InternalModel);
            }
            else
            {
                container.Payload.Insert(index, Item.InternalModel);
            }
        }

        private void MoveItem(InventoryContainerVM value)
        {
            if(value == null)
            {
                return;
            }

            Container oldContainer = _inventory.Items.FirstOrDefault(x =>
                x is Container container && container.Payload.Contains(Item.InternalModel)
            ) as Container;
            Container newContainer = value.OriginalModelRefernece;

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
        public string Text { get; set; }

        public override string ToString() => Text;
    }

    public class InventoryContainerVM
    {
        public Container OriginalModelRefernece { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}