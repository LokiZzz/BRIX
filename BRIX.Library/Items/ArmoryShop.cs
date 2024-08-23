using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Items;
using BRIX.Utility.Extensions;
using static BRIX.Library.Items.ArtifactExtensions;

namespace BRIX.Library.Items
{
    /// <summary>
    /// Магазин оружия. Генератор брони и оружия, которые смогут приобрести игроки.
    /// </summary>
    public class ArmoryShop
    {
        /// <summary>
        /// Названия оружия.
        /// </summary>
        public List<string> WeaponNames { get; set; } = [];

        /// <summary>
        /// Названия дальнобойного оружия.
        /// </summary>
        public List<string> RangedWeaponNames { get; set; } = [];

        /// <summary>
        /// Приставки для оружия, означающие его грейд, чем выше индекс имени, тем круче оружие.
        /// </summary>
        public List<string> WeaponGradesNames { get; set; } = [];

        /// <summary>
        /// Постфиксы для дополнительной уникальности.
        /// </summary>
        public List<string> WeaponNarrativePrefixes { get; set; } = [];

        /// <summary>
        /// Названия брони.
        /// </summary>
        public List<string> ArmorNames { get; set; } = [];

        /// <summary>
        /// Приставки для брони, означающие его грейд, чем выше индекс имени, тем круче броня.
        /// </summary>
        public List<string> ArmorGradesNames { get; set; } = [];

        /// <summary>
        /// Постфиксы брони для дополнительной уникальности.
        /// </summary>
        public List<string> ArmorNarrativePrefixes { get; set; } = [];

        /// <summary>
        /// Сгенерировать ассортимент оружия с небольшим разбросом уровня относительно заданного.
        /// </summary>
        /// <returns></returns>
        public List<Artifact> GenerateWeapons(int meleeCount, int rangedCount, int level, int gradeStep)
        {
            List<Artifact> weapons = [];

            foreach (int itemNumber in Enumerable.Range(0, meleeCount + rangedCount))
            {
                Artifact weapon = new();
                int lowPrice = CharacterCalculator.GetExpForLevel(level - 1) * 4;
                int highPrice = CharacterCalculator.GetExpForLevel(level + 1) * 4;
                int price = new Random().Next(lowPrice, highPrice);
                int distance = itemNumber > meleeCount - 1 ? new Random().Next(2, 15) : 1;
                weapon.Distance = distance;
                weapon.TuneToPrice(price, EArtifactTuneStrategy.ByDamage);
                weapon.Name = GetWeaponName(gradeStep, price, distance);

                weapons.Add(weapon);
            }

            return [.. weapons.OrderBy(x => x.Distance)];
        }

        /// <summary>
        /// Сгенерировать ассортимент оружия с небольшим разбросом уровня относительно заданного.
        /// </summary>
        /// <returns></returns>
        public List<Artifact> GenerateArmor(int count, int level, int gradeStep)
        {
            List<Artifact> armor = [];

            foreach (int itemNumber in Enumerable.Range(0, count))
            {
                Artifact armorItem = new();
                int lowPrice = CharacterCalculator.GetExpForLevel(level - 1) * 4;
                int highPrice = CharacterCalculator.GetExpForLevel(level + 1) * 4;
                int price = new Random().Next(lowPrice, highPrice);
                armorItem.TuneToPrice(price, EArtifactTuneStrategy.ByDefense);
                armorItem.Name = GetArmorName(gradeStep, price);

                armor.Add(armorItem);
            }

            return [.. armor.OrderBy(x => x.Defense.Average())];
        }

        /// <summary>
        /// Сгенерировать ассортимент оружия с небольшим разбросом уровня относительно заданного.
        /// </summary>
        /// <returns></returns>
        private string GetWeaponName(int gradeStep, int price, int distance)
        {
            string weaponName = distance == 1 ? WeaponNames.Random() : RangedWeaponNames.Random();
            string prefix = ToRightDeclension(WeaponNarrativePrefixes.Random(), weaponName);
            string grade = string.Empty;

            if (price > gradeStep)
            {
                int gradeIndex = price / gradeStep - 1;
                grade = gradeIndex < WeaponGradesNames.Count
                    ? WeaponGradesNames[gradeIndex]
                    : WeaponGradesNames.Last();
                grade = ToRightDeclension(grade, weaponName);
            }

            weaponName = prefix + " " + weaponName;

            if(!string.IsNullOrEmpty(grade))
            {
                weaponName = grade + " " + weaponName;
            }

            weaponName = string.Concat(weaponName[..1].ToUpper(), weaponName.ToLower().AsSpan(1));

            return weaponName;
        }

        private string GetArmorName(int gradeStep, int price)
        {
            string name = ArmorNames.Random();
            string prefix = ToRightDeclension(ArmorNarrativePrefixes.Random(), name);
            string grade = string.Empty;

            if (price > gradeStep)
            {
                int gradeIndex = price / gradeStep - 1;
                grade = gradeIndex < ArmorGradesNames.Count
                    ? ArmorGradesNames[gradeIndex]
                    : ArmorGradesNames.Last();
                grade = ToRightDeclension(grade, name);
            }

            name = prefix + " " + name;

            if (!string.IsNullOrEmpty(grade))
            {
                name = grade + " " + name;
            }

            name = string.Concat(name[..1].ToUpper(), name.ToLower().AsSpan(1));

            return name;
        }

        public static string ToRightDeclension(string input, string dependency)
        {
            char[] letters1 = ['а', 'у', 'э', 'ю', 'я'];

            if (letters1.Contains(dependency.Last()))
            {
                input = string.Concat(input.AsSpan(0, input.Length - 2), "ая");
            }

            char[] letters2 = ['о', 'е', 'ё'];

            if (letters2.Contains(dependency.Last()))
            {
                input = string.Concat(input.AsSpan(0, input.Length - 2), "ое");
            }

            char[] letters3 = ['и', 'ы'];

            if (letters3.Contains(dependency.Last()))
            {
                input = string.Concat(input.AsSpan(0, input.Length - 2), "ые");
            }

            return input;
        }
    }

    public static class ArtifactExtensions
    {
        public enum EArtifactTuneStrategy
        {
            ByDamage = 0,
            ByDefense = 1
        }

        public static void TuneToPrice(this Artifact artifact, int price, EArtifactTuneStrategy strategy)
        {
            DicePool dicePoolToTune = strategy switch
            {
                EArtifactTuneStrategy.ByDamage => artifact.Damage,
                EArtifactTuneStrategy.ByDefense => artifact.Defense,
                _ => throw new Exception("Неизвестная стратегия подстройки")
            };

            dicePoolToTune.Dice.Clear();
            dicePoolToTune.Modifier = 1;

            // Прибавляем по 1 к среднему, пока не достигнем желаемой стоимости.
            while (artifact.Price < price)
            {
                dicePoolToTune.Modifier++;
            }

            if (dicePoolToTune.Modifier >= 2)
            {
                int upperPrice = artifact.Price;
                dicePoolToTune.Modifier--;
                int lowerPrice = artifact.Price;

                if (upperPrice - price <= price - lowerPrice)
                {
                    dicePoolToTune.Modifier++;
                }
            }

            if (dicePoolToTune.Average() > 3)
            {
                DicePool spread = DicePool.FromValue(dicePoolToTune.Average(), 0.5);
                dicePoolToTune.Dice = spread.Dice;
                dicePoolToTune.Modifier = spread.Modifier;
            }
        }
    }
}