using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Inventory;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Inventory;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterInventoryPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;
        private Character _currentCharacter;

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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Library.Characters.Inventory updatedInventory =
                query.GetParameterOrDefault<Library.Characters.Inventory>(NavigationParameters.Inventory);

            if(updatedInventory != null)
            {
                // Handle back from editing
            }
        }

        private async Task Initialize(bool force = false) 
        {
            if(InventoryItems == null || force)
            {
                _currentCharacter = await _characterService.GetCurrentCharacter();

                Library.Characters.Inventory testInventory = new()
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
                if(!_currentCharacter.Inventory.Content.Any())
                {
                    _currentCharacter.Inventory = testInventory;
                    await _characterService.UpdateAsync(_currentCharacter);
                }

                InventoryItems = new(_currentCharacter.Inventory.Content.Select(ToVM).ToList());
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

            bool saveContent = false;

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

                saveContent = resultDeleteContent.Answer == EAlertPopupResult.No;
            }

            _currentCharacter.Inventory.Remove(item.OriginalModelReference, saveContent);
            await _characterService.UpdateAsync(_currentCharacter);
            await Initialize(force: true);
        }

        [RelayCommand]
        public async Task Edit(InventoryItemVM item)
        {
            await Navigation.NavigateAsync<AddOrEditInventoryItemPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.Inventory, _currentCharacter.Inventory),
                (NavigationParameters.InventoryItem, item.OriginalModelReference)
            );
        }

        [RelayCommand]
        public async Task New()
        {
            await Navigation.NavigateAsync<AddOrEditInventoryItemPage>(
                (NavigationParameters.EditMode, EEditingMode.Add),
                (NavigationParameters.Inventory, _currentCharacter.Inventory)
            );
        }

        private ImageSource _gemIS;
        private ImageSource _containerIS;
        private ImageSource _equipmentIS;
        private ImageSource _consumableIS;

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
}
