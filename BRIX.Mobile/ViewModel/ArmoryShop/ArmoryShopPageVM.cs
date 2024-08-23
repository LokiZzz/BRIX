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
                OnPropertyChanged(nameof(ArtifactPrice));
                OnPropertyChanged(nameof(ArtifactLevel));
            }
        }

        private int _weaponDistance = 1;
        public int WeaponDistance
        {
            get => _weaponDistance;
            set
            {
                SetProperty(ref _weaponDistance, value);
                OnPropertyChanged(nameof(ArtifactPrice));
                OnPropertyChanged(nameof(ArtifactLevel));
            }
        }

        public int ArtifactPrice => GetTheoreticalArtifact().Price;

        public int ArtifactLevel => GetTheoreticalArtifact().Level;

        private Artifact GetTheoreticalArtifact()
        {
            Artifact artifact = new() { Distance = WeaponDistance };

            if (DicePool.TryParse(WeaponDice, out DicePool? damage) && damage != null)
            {
                artifact.Damage = damage;
            }

            if (DicePool.TryParse(ArmorDice, out DicePool? armor) && armor != null)
            {
                artifact.Damage = armor;
            }

            return artifact;
        }

        private string _armorDice = "1d4";
        public string ArmorDice
        {
            get => _armorDice;
            set
            {
                SetProperty(ref _armorDice, value);
                OnPropertyChanged(nameof(ArtifactPrice));
                OnPropertyChanged(nameof(ArtifactLevel));
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
            set => SetProperty(ref _armor, value);
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
            Library.Items.ArmoryShop shop = new()
            {
                WeaponNames = [
                    "Посох", "Булава", "Дубинка", "Кинжал", "Копьё", "Молот",
                    "Палица", "Топор", "Серп", "Алебарда", "Хопеш", "Кирка", "Глефа",
                    "Меч", "Моргенштерн", "Пика", "Рапира", "Секира", "Скимитар",
                    "Ятаган", "Трезубец", "Цеп", "Двуручный меч", "Цвайхендер", "Катана",
                    "Коса", "Лопата"
                ],
                RangedWeaponNames = [
                    "Лук", "Арбалет", "Мушкетон", "Винтовка", "Пистоль", "Праща",
                    "Пушка", "Аркебуза", "Ружьё", "Жезл", "Посох", "Волшебная палочка",
                    "Кристалл"
                ],
                ArmorNames = [
                    "Шлем", "Шляпа", "Плащ", "Перчатки", "Рукавицы", "Сапоги", "Сабатоны", "Ботинки", "Сандалии",
                    "Поножи", "Наручи", "Кираса", "Доспех", "Бригантина", "Куртка", "Гульфик", "Браслеты",
                    "Щит", "Баклер", "Башенный щит", "Щитки", "Кольчуга", "Шкура неопознанного зверя",
                    "Рубаха", "Мантия", "Панцирь", "Переносная баррикада", "Дверь"
                ],
                WeaponGradesNames = ["Хороший", "Качественный", "Дорогой", "Отличный", "Редкий", "Легендарный"],
                    WeaponNarrativePrefixes = [
                    "Кованый", "Рунический", "Волшебный", "Крепкий", "Сэлоранский", "Аварисский",
                    "Огненный", "Зачарованный", "Усиленный", "Композитный", "Украшенный",
                    "Палеомантический", "Морозный", "Каменный", "Кристальный", "Железный", "Электрический",
                    "Раскатный", "Лёгкий", "Тяжёлый", "Ядовитый", "Кровожадный", "Бронебойный"
                ],
                ArmorNarrativePrefixes = [
                    "Кованый", "Рунический", "Волшебный", "Крепкий", "Сэлоранский", "Аварисский",
                    "Зачарованный", "Усиленный", "Композитный", "Украшенный",
                    "Морозный", "Каменный", "Кристальный", "Железный", "Лёгкий", "Тяжёлый", "Кожаный", "Кольчужный",
                    "Латный", "Чешуйчатый", "Пластинчатый", "Костяной"
                ]
            };
            shop.ArmorGradesNames = shop.WeaponGradesNames;

            List<Artifact> weapons = shop.GenerateWeapons(Melee, Ranged, Level, GradeStep);
            List<Artifact> armor = shop.GenerateArmor(Armor, Level, GradeStep);

            List<ShopItemVM> items = weapons.Select(x => new ShopItemVM(x)).ToList();
            items.AddRange(armor.Select(x => new ShopItemVM(x)));
            GeneratedItems = new(items);
            Preferences.Set(_preferencesKey, JsonConvert.SerializeObject(GeneratedItems) ?? string.Empty);
        }

        private const string _preferencesKey = "ShopItems";

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

        public ShopItemVM(Artifact item)
        {
            Name = item.Name;
            Price = item.Price.ToString();

            if (item.Damage.Average() > 0)
            {
                Stats = $"{item.Damage} ({item.Distance} m), {item.Level} Lvl";
                Icon = item.Distance > 1 ? AwesomeRPG.Crossbow : AwesomeRPG.Axe;
            }
            else if(item.Defense.Average() > 0)
            {
                Stats = $"{item.Defense}, {item.Level} Lvl";
                Icon = AwesomeRPG.Vest;
            }
        }
        
        public string Icon { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Stats { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
    }
}
