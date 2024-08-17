using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;
using System.Collections.Generic;
using System.Diagnostics;

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
        public List<WeaponItem> GenerateWeapons(int count, int level, int gradeStep)
        {
            List<WeaponItem> weapons = [];

            foreach (int itemNumber in Enumerable.Range(0, count))
            {
                WeaponItem weapon = new();
                int lowPrice = CharacterCalculator.GetExpForLevel(level - 1) * 4;
                int highPrice = CharacterCalculator.GetExpForLevel(level + 1) * 4;
                int price = new Random().Next(lowPrice, highPrice);
                int distance = itemNumber % 2 == 0 ? 1 : new Random().Next(2, 15);
                weapon.TuneToPrice(price, distance);
                weapon.Name = GetWeaponName(gradeStep, price, distance);

                weapons.Add(weapon);
            }

            return weapons.OrderBy(x => x.Distance).ToList();
        }

        /// <summary>
        /// Сгенерировать ассортимент оружия с небольшим разбросом уровня относительно заданного.
        /// </summary>
        /// <returns></returns>
        public List<ArmorItem> GenerateArmor(int count, int level, int gradeStep)
        {
            List<ArmorItem> armor = [];

            foreach (int itemNumber in Enumerable.Range(0, count))
            {
                ArmorItem armorItem = new();
                int lowPrice = CharacterCalculator.GetExpForLevel(level - 1) * 4;
                int highPrice = CharacterCalculator.GetExpForLevel(level + 1) * 4;
                int price = new Random().Next(lowPrice, highPrice);
                armorItem.TuneToPrice(price);
                armorItem.Name = GetArmorName(gradeStep, price);

                armor.Add(armorItem);
            }

            return armor.OrderBy(x => x.Defense.Average()).ToList();
        }

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
                weaponName = weaponName = grade + " " + weaponName;
            }

            weaponName = weaponName.Substring(0, 1).ToUpper() + weaponName.ToLower().Substring(1);

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
                name = name = grade + " " + name;
            }

            name = name.Substring(0, 1).ToUpper() + name.ToLower().Substring(1);

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

}
