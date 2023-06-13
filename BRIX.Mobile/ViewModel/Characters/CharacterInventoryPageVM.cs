using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Characters
{
    public class CharacterInventoryPageVM : ViewModelBase
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
                        new InventoryItem { Name = "Ремень с медной бляшкой" },
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
                        new Equipment { Name = "Фамильный меч", CoinsPrice = 100 },
                    }
                };

                InventoryItems = new(inventory.Items.Select(ToVM).ToList());
            }
        }

        private InventoryItemVM ToVM(InventoryItem item)
        {
            InventoryItemVM viewModel = new() 
            { 
                Name = item.Name,
                Count = item.Count,
            };

            switch (item)
            {
                case Container container:
                    viewModel.Type = Localization.InventoryItemContainer;
                    viewModel.Payload = container.Payload.Select(ToVM).ToList();
                    break;
                case Equipment equipment:
                    viewModel.Type = Localization.InventoryItemEquipment;
                    viewModel.Price = equipment.CoinsPrice;
                    break;
                case Consumable consumable:
                    viewModel.Type = Localization.InventoryItemConsumable;
                    viewModel.Price = consumable.CoinsPrice;
                    break;
                case InventoryItem:
                    viewModel.Type = Localization.InventoryItem;
                    break;
            }

            return viewModel;
        }
    }

    public partial class InventoryItemVM : ObservableObject
    {
        private Random _random = new Random();
        public Color Color => Color.FromRgb(_random.Next(256), _random.Next(256), _random.Next(256));

        public string Name { get; set; }

        public string Type { get; set; }

        public List<InventoryItemVM> Payload { get; set; } = new();

        public bool ShowPayload => Type == Localization.InventoryItemContainer;

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                SetProperty(ref _count, value);
                OnPropertyChanged(nameof(ShowPrice));
            }
        }

        public bool ShowPrice => Type == Localization.InventoryItemEquipment 
            || Type == Localization.InventoryItemConsumable;


        private int _price;
        public int Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                OnPropertyChanged(nameof(ShowCount));
            }
        }

        public bool ShowCount => Count != 1 
            || Type == Localization.InventoryItemConsumable;
    }
}
