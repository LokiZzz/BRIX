using BRIX.Library.DiceValue;
using BRIX.Library.Items;

//while(true)
//{
//    string[] input = Console.ReadLine().Split(' ');
//    int price = int.Parse(input[0]);
//    int distance = int.Parse(input[1]);
//    WeaponItem sword = new();
//    sword.TuneToPrice(price, distance);

//    Console.WriteLine($"Damage: {sword.Damage}, Distance: {sword.Distance}, Price: {sword.Price}, Level: {sword.LevelRequired}");
//}

ArmoryShop shop = new ();
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
shop.WeaponGradesNames = ["Хороший", "Качественный", "Отличный", "Редкий", "Легендарный"];
shop.WeaponNarrativePrefixes = [
    "Драконий", "Кованый", "Рунический", "Волшебный", "Крепкий", "Сэлоранский", "Аварисский",
    "Огненный", "Зачарованный", "Усиленный", "Композитный", "Дорогой", "Украшенный", 
    "Палеомантический", "Морозный", "Каменный", "Кристальный", "Железный", "Электрический",
    "Раскатный", "Лёгкий", "Тяжёлый", "Ядовитый", "Кровожадный"
];

List<WeaponItem> weapons = shop.GenerateWeapons(15, 1, 10, 2000);

Console.WriteLine($"\t\t\t\tУрон\tДистанция\tСтоимость");

foreach (WeaponItem weapon in weapons)
{
    Console.WriteLine(weapon.Name);
    Console.WriteLine($"\t\t\t\t{weapon.Damage}\t{weapon.Distance}\t{weapon.Price} монет");
}

Console.ReadLine();