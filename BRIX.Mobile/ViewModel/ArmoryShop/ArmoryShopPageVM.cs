using BRIX.Library.DiceValue;
using BRIX.Library.Items;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.ArmoryShop
{
    public partial class ArmoryShopPageVM : ViewModelBase
    {
        private string _weaponDice = "1d4";
        public string WeaponDice
        {
            get => _weaponDice;
            set
            {
                SetProperty(ref _weaponDice, value);
                OnPropertyChanged(nameof(WeaponPrice));
                OnPropertyChanged(nameof(WeaponLevel));
            }
        }

        private int _weaponDistance = 1;
        public int WeaponDistance
        {
            get => _weaponDistance;
            set
            {
                SetProperty(ref _weaponDistance, value);
                OnPropertyChanged(nameof(WeaponPrice));
                OnPropertyChanged(nameof(WeaponLevel));
            }
        }

        public int WeaponPrice
        {
            get
            {
                if(DicePool.TryParse(WeaponDice, out DicePool? dices) && dices != null)
                {
                    return new WeaponItem { Damage = dices, Distance = WeaponDistance }.Price;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int WeaponLevel
        {
            get
            {
                if (DicePool.TryParse(WeaponDice, out DicePool? dices) && dices != null)
                {
                    return new WeaponItem { Damage = dices, Distance = WeaponDistance }.LevelRequired;
                }
                else
                {
                    return 0;
                }
            }
        }

        private string _armorDice = "1d4";
        public string ArmorDice
        {
            get => _armorDice;
            set
            {
                SetProperty(ref _armorDice, value);
                OnPropertyChanged(nameof(ArmorPrice));
                OnPropertyChanged(nameof(ArmorLevel));
            }
        }

        public int ArmorPrice
        {
            get
            {
                if (DicePool.TryParse(ArmorDice, out DicePool? dices) && dices != null)
                {
                    return new ArmorItem { Defense = dices }.Price;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int ArmorLevel
        {
            get
            {
                if (DicePool.TryParse(ArmorDice, out DicePool? dices) && dices != null)
                {
                    return new ArmorItem { Defense = dices }.LevelRequired;
                }
                else
                {
                    return 0;
                }
            }
        }

        private int _melee = 5;
        public int Melee
        {
            get => _melee;
            set => SetProperty(ref _melee, value);
        }

        private int _ranged = 5;
        public int Ranged
        {
            get => _ranged;
            set => SetProperty(ref _ranged, value);
        }

        private int _armor = 5;
        public int Armor
        {
            get => _armor;
            set => SetProperty(ref _ranged, value);
        }

        private int _level = 2;
        public int Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        private int _gradeStep = 1500;
        public int GradeStep
        {
            get => _gradeStep;
            set => SetProperty(ref _gradeStep, value);
        }

        private ObservableCollection<ShopItemVM> _generatedItems = [];
        public ObservableCollection<ShopItemVM> GeneratedItems
        {
            get => _generatedItems;
            set => SetProperty(ref _generatedItems, value);
        }

        [RelayCommand]
        public void Generate()
        {
            Library.Items.ArmoryShop shop = new();
            shop.WeaponNames = [
                "Посох", "Булава", "Дубинка", "Кинжал", "Копьё", "Молот",
                "Палица", "Топор", "Серп", "Алебарда", "Хопеш", "Кирка", "Глефа",
                "Меч", "Моргенштерн", "Пика", "Рапира", "Секира", "Скимитар",
                "Ятаган", "Трезубец", "Цеп", "Двуручный меч", "Цвайхендер", "Катана",
                "Коса", "Лопата"
            ];
            shop.RangedWeaponNames = [
                "Лук", "Арбалет", "Мушкетон", "Винтовка", "Пистоль", "Праща",
                "Пушка", "Аркебуза", "Ружьё", "Жезл", "Посох", "Волшебная палочка",
                "Кристалл"
            ];
            shop.ArmorNames = [
                "Шлем", "Шляпа", "Плащ", "Перчатки", "Рукавицы", "Сапоги", "Сабатоны", "Ботинки", "Сандалии",
                "Поножи", "Наручи", "Кираса", "Доспех", "Бригантина", "Куртка", "Гульфик", "Браслеты",
                "Щит", "Баклер", "Башенный щит", "Щитки", "Кольчуга", "Шкура неопознанного зверя",
                "Рубаха", "Мантия", "Панцирь", "Переносная баррикада", "Дверь"
            ];
            shop.WeaponGradesNames = ["Хороший", "Качественный", "Дорогой", "Отличный", "Редкий", "Легендарный"];
            shop.ArmorGradesNames = shop.WeaponGradesNames;
            shop.WeaponNarrativePrefixes = [
                "Кованый", "Рунический", "Волшебный", "Крепкий", "Сэлоранский", "Аварисский",
                "Огненный", "Зачарованный", "Усиленный", "Композитный", "Украшенный",
                "Палеомантический", "Морозный", "Каменный", "Кристальный", "Железный", "Электрический",
                "Раскатный", "Лёгкий", "Тяжёлый", "Ядовитый", "Кровожадный", "Бронебойный"
            ];
            shop.ArmorNarrativePrefixes = [
                "Кованый", "Рунический", "Волшебный", "Крепкий", "Сэлоранский", "Аварисский",
                "Зачарованный", "Усиленный", "Композитный", "Украшенный",
                "Морозный", "Каменный", "Кристальный", "Железный", "Лёгкий", "Тяжёлый", "Кожаный", "Кольчужный",
                "Латный", "Чешуйчатый", "Пластинчатый", "Костяной"
            ];

            List<WeaponItem> weapons = shop.GenerateWeapons(Melee, Ranged, Level, GradeStep);
            List<ArmorItem> armor = shop.GenerateArmor(Armor, Level, GradeStep);

            List<ShopItemVM> items = weapons.Select(x => new ShopItemVM(x)).ToList();
            items.AddRange(armor.Select(x => new ShopItemVM(x)));
            GeneratedItems = new(items);
            Preferences.Set(_preferencesKey, JsonConvert.SerializeObject(GeneratedItems));
        }

        private string _preferencesKey = "ShopItems";

        public override Task OnNavigatedAsync()
        {
            if (GeneratedItems.Count == 0)
            {
                string jsonItems = Preferences.Get(_preferencesKey, string.Empty);

                if (!string.IsNullOrEmpty(jsonItems))
                {
                    GeneratedItems = JsonConvert.DeserializeObject<ObservableCollection<ShopItemVM>>(jsonItems) ?? [];
                }
            }

            return Task.CompletedTask;
        }
    }

    public class ShopItemVM
    {
        public ShopItemVM() { }

        public ShopItemVM(WeaponItem item)
        {
            Name = item.Name;
            Stats = $"{item.Damage} ({item.Distance} m), {item.LevelRequired} Lvl";
            Price = item.Price.ToString();
            Icon = item.Distance > 1 ? AwesomeRPG.Crossbow : AwesomeRPG.Axe;
        }

        public ShopItemVM(ArmorItem item)
        {
            Name = item.Name;
            Stats = $"{item.Defense}, {item.LevelRequired} Lvl";
            Price = item.Price.ToString();
            Icon = AwesomeRPG.Vest;
        }
        
        public string Icon { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Stats { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
    }
}
