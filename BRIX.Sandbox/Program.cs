using BRIX.Library.DiceValue;
using BRIX.Library.Items;

//while (true)
//{
//    //Price test
//    string[] input = Console.ReadLine().Split(' ');
//    DicePool.TryParse(input[0], out DicePool damage);
//    int distance = int.Parse(input[1]);
//    WeaponItem sword = new() { Damage = damage, Distance = distance };

//    Console.WriteLine($"Damage: {sword.Damage}, Distance: {sword.Distance}, Price: {sword.Price}, Level: {sword.LevelRequired}");

//    ////Tune test
//    //string[] input = Console.ReadLine().Split(' ');
//    //int price = int.Parse(input[0]);
//    //int distance = int.Parse(input[1]);
//    //WeaponItem sword = new();
//    //sword.TuneToPrice(price, distance);

//    //Console.WriteLine($"Damage: {sword.Damage}, Distance: {sword.Distance}, Price: {sword.Price}, Level: {sword.LevelRequired}");
//}

ArmoryShop shop = new();
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

//List<ArmorItem> items = shop.GenerateArmor(10, 1, 2000);

//Console.WriteLine($"\t\t\t\tУрон\tСтоимость\t");

//foreach (ArmorItem item in items)
//{
//    Console.WriteLine("________________________________________________________________________________");
//    Console.WriteLine(item.Name);
//    Console.WriteLine($"\t\t\t\t{item.Defense}\t{item.Price} монет\t{item.LevelRequired}");
//    Console.WriteLine();
//}

//Console.ReadLine();