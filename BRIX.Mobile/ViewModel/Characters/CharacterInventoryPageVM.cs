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

            InitializeVisual();
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
                Character currentCharacter = await _characterService.GetCurrentCharacter();

                Inventory testInventory = new()
                {
                    Coins = 250,
                    Content = new List<InventoryItem>
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
                if(!currentCharacter.Inventory.Content.Any())
                {
                    currentCharacter.Inventory = testInventory;
                    await _characterService.UpdateAsync(currentCharacter);
                }

                InventoryItems = new(currentCharacter.Inventory.Content.Select(ToVM).ToList());
            }
        }

        private bool isDarkBackgroundNow = true;
        private Color _darkItemColor;
        private Color _lightItemColor;

        private InventoryItemVM ToVM(InventoryItem item)
        {
            InventoryItemVM viewModel = new() 
            { 
                Name = item.Name,
                Count = item.Count,
                BackgroundColor = isDarkBackgroundNow ? _darkItemColor : _lightItemColor,
                Description = item.Description,
                OriginalModelReference = item
            };

            switch (item)
            {
                case Container container:
                    viewModel.Type = EInventoryItemType.Container;
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Payload = new (container.Payload.Select(ToVM));
                    isDarkBackgroundNow = !isDarkBackgroundNow;
                    viewModel.Icon = _containerIS;
                    break;
                case Equipment equipment:
                    viewModel.Type = EInventoryItemType.Equipment;
                    viewModel.Price = equipment.CoinsPrice;
                    viewModel.Icon = _equipmentIS;
                    break;
                case Consumable consumable:
                    viewModel.Type = EInventoryItemType.Consumable;
                    viewModel.Price = consumable.CoinsPrice;
                    viewModel.Icon = _consumableIS;
                    break;
                case InventoryItem:
                    viewModel.Type = EInventoryItemType.Thing;
                    viewModel.Icon = _gemIS;
                    break;
            }

            return viewModel;
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

            if(result.Answer == EAlertPopupResult.No)
            {
                return;
            }

            if(item.Type == EInventoryItemType.Container && item.Payload.Any())
            {
                AlertPopupResult resultDeleteContent = 
                    await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(new AlertPopupParameters
                    {
                        Mode = EAlertMode.AskYesOrNo,
                        YesText = Localization.Yes,
                        NoText = Localization.No,
                        Title = Localization.Warning,
                        Message = string.Format(Localization.AskDeleteContainerWithContent, item.Name),
                    }
                );

                if(resultDeleteContent.Answer == EAlertPopupResult.No)
                {
                    foreach(InventoryItemVM payloadItem in item.Payload)
                    {
                        InventoryItems.Add(payloadItem);
                    }
                }
            }

            foreach (InventoryItemVM inventoryItem in InventoryItems)
            {
                SearchAndDelete(inventoryItem);
            }
        }

        private void SearchAndDelete(InventoryItemVM item)
        {
            foreach(InventoryItemVM searchingItem in item.Payload)
            {

            }
        }

        public ImageSource _gemIS;
        public ImageSource _containerIS;
        public ImageSource _equipmentIS;
        public ImageSource _consumableIS;

        private void InitializeVisual()
        {
            Application.Current.Resources.TryGetValue("BRIXMedium", out object darkItemColor);
            _darkItemColor = darkItemColor as Color;
            Application.Current.Resources.TryGetValue("BRIXDim", out object lightItemColor);
            _lightItemColor = lightItemColor as Color;
            _gemIS = ImageSource.FromFile("Inventory/gem.svg");
            _containerIS = ImageSource.FromFile("Inventory/chest.svg");
            _equipmentIS = ImageSource.FromFile("Inventory/blade.svg");
            _consumableIS = ImageSource.FromFile("Inventory/arrow.svg");
        }
    }

    public partial class InventoryItemVM : ObservableObject
    {
        public Color BackgroundColor { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ImageSource Icon { get; set; }

        private EInventoryItemType _type;
        public EInventoryItemType Type
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

        public bool ShowPayload => Type == EInventoryItemType.Container;

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

        public bool ShowCount => Count != 1 || Type == EInventoryItemType.Consumable;
        

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

        public bool ShowPrice => Type == EInventoryItemType.Equipment || Type == EInventoryItemType.Consumable;

        private RelayCommand _descriptionCommand;
        public RelayCommand DescriptionCommand
        {
            get => _descriptionCommand;
            set => SetProperty(ref _descriptionCommand, value);
        }

        public InventoryItem OriginalModelReference { get; set; }
    }

    public enum EInventoryItemType
    {
        Thing,
        Container,
        Equipment,
        Consumable
    }
}
