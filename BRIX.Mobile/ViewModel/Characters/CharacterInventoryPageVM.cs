using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterInventoryPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;

        public CharacterInventoryPageVM(ICharacterService characterService)
        {
            WeakReferenceMessenger.Default.Register<CurrentCharacterChanged>(
                this,
                async (r, m) => await Initialize(true)
            );
            _characterService = characterService;
        }

        private ObservableCollection<InventoryItemVM> _inventoryItems;
        public ObservableCollection<InventoryItemVM> InventoryItems
        {
            get => _inventoryItems;
            set => SetProperty(ref _inventoryItems, value);
        }

        public override async Task OnNavigatedAsync()
        {
            await Initialize(false);
        }

        private async Task Initialize(bool force = false) 
        {
            if(InventoryItems == null || force)
            {
                //Character currentCharacter = await _characterService.GetCurrentCharacter();
                //InventoryItems = new(currentCharacter.Inventory.Items.Select(ToVM).ToList());
                Inventory inventory = new()
                {
                    Coins = 250,
                    Items = new List<InventoryItem>
                    {
                        new InventoryItem { Name = "Ремень с медной пряжкой", Description = "Пояс украшен металлической фурнитурой различного размера. При желании пряжка отстегивается." },
                        new InventoryItem { Name = "Стильные сапоги" },
                        new Container 
                        {
                            Name = "Рюкзак",
                            Payload = new List<InventoryItem>
                            {
                                new InventoryItem { Name = "Расчёска" },
                                new InventoryItem { Name = "Бутерброд", Count = 5 },
                                new InventoryItem { Name = "Кремень и кресало" },
                                new InventoryItem { Name = "Мини-палатка" },
                                new Container
                                {
                                    Name = "Шкатулка",
                                    Payload = new List<InventoryItem>
                                    {
                                        new InventoryItem { Name = "Катушка ниток", Count = 10 },
                                        new InventoryItem { Name = "Игла", Count = 4 },
                                    }
                                },
                            }
                        },
                        new Container
                        {
                            Name = "Поясные сумки",
                            Payload = new List<InventoryItem>
                            {
                                new Consumable { Name = "Металлический шарик", Count = 100, CoinsPrice = 1 },
                                new InventoryItem { Name = "Расчёска" },
                            }
                        },
                        new Equipment { Name = "Фамильный меч", Description = "Азот (Azoth) — магический меч великого лекаря (по легендам). Азот — это имя демона, заключённого в кристалл, использованный в эфесе этого оружия.", CoinsPrice = 100 },
                    }
                };

                InventoryItems = new(inventory.Items.Select(ToVM).ToList());
            }
        }

        private bool isDarkBackgroundNow = true;

        private InventoryItemVM ToVM(InventoryItem item)
        {
            string resourceKey = isDarkBackgroundNow ? "BRIXMedium" : "BRIXDim";
            Application.Current.Resources.TryGetValue(resourceKey, out object colorObject);

            InventoryItemVM viewModel = new() 
            { 
                Name = item.Name,
                Count = item.Count,
                BackgroundColor = colorObject as Color,
                Description = item.Description,
            };

            switch (item)
            {
                case Container container:
                    viewModel.Type = InventoryItemType.Container;
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Payload = new (container.Payload.Select(ToVM));
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    break;
                case Equipment equipment:
                    viewModel.Type = InventoryItemType.Equipment;
                    viewModel.Price = equipment.CoinsPrice;
                    break;
                case Consumable consumable:
                    viewModel.Type = InventoryItemType.Consumable;
                    viewModel.Price = consumable.CoinsPrice;
                    break;
                case InventoryItem:
                    viewModel.Type = InventoryItemType.Thing;
                    break;
            }

            viewModel.Icon = GetItemTypeIcon(viewModel.Type);

            return viewModel;
        }

        private string GetItemTypeIcon(InventoryItemType type)
        {
            switch (type)
            {
                case InventoryItemType.Thing:
                    return "Inventory/gem.svg";
                case InventoryItemType.Container:
                    return "Inventory/chest.svg";
                case InventoryItemType.Equipment:
                    return "Inventory/blade.svg";
                case InventoryItemType.Consumable:
                    return "Inventory/arrow.svg";
            }

            return string.Empty;
        }

        [RelayCommand]
        public async Task ShowDescription(InventoryItemVM item)
        {
            await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                new AlertPopupParameters 
                {
                    Mode = EAlertMode.ShowMessage,
                    OkText = Localization.Ok,
                    Title = item.Name,
                    Message = string.IsNullOrEmpty(item.Description) ? $"{item.Name}..." : item.Description,
                }
            );
        }

        [RelayCommand]
        public async Task Delete(InventoryItemVM item)
        {
            AlertPopupResult result = await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(
                new AlertPopupParameters
                {
                    Mode = EAlertMode.AskYesOrNo,
                    YesText = Localization.Yes,
                    NoText = Localization.No,
                    Title = Localization.Warning,
                    Message = string.Format(Localization.AskToDeleteInventoryItem, item.Name),
                }
            );
        }
    }

    public partial class InventoryItemVM : ObservableObject
    {
        public Color BackgroundColor { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        private InventoryItemType _type;
        public InventoryItemType Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                OnPropertyChanged(nameof(ShowCount));
                OnPropertyChanged(nameof(ShowPrice));
                OnPropertyChanged(nameof(ShowPayload));
            }
        }

        public ObservableCollection<InventoryItemVM> Payload { get; set; } = new();

        public bool ShowPayload => Type == InventoryItemType.Container;

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                SetProperty(ref _count, value);
                OnPropertyChanged(nameof(ShowCount));
            }
        }

        public bool ShowCount => Count != 1 || Type == InventoryItemType.Consumable;
        

        private int _price;
        public int Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                OnPropertyChanged(nameof(ShowPrice));
            }
        }

        public bool ShowPrice => Type == InventoryItemType.Equipment || Type == InventoryItemType.Consumable;

        private RelayCommand _descriptionCommand;
        public RelayCommand DescriptionCommand
        {
            get => _descriptionCommand;
            set => SetProperty(ref _descriptionCommand, value);
        }
    }

    public enum InventoryItemType
    {
        Thing,
        Container,
        Equipment,
        Consumable
    }
}
