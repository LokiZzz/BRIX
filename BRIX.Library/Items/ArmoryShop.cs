using BRIX.Library.Characters;
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
        /// Сгенерировать ассортимент оружия.
        /// </summary>
        /// <returns></returns>
        public List<WeaponItem> GenerateWeapons(int count, int minimumLevel, int maximumLevel, int gradeStep)
        {
            List<WeaponItem> weapons = [];

            foreach (int itemNumber in Enumerable.Range(0, count))
            {
                int level = new Random().Next(minimumLevel, maximumLevel);
                WeaponItem weapon = new ();
                int price = CharacterCalculator.GetExpForLevel(level * 2);
                int distance = itemNumber % 2 == 0 ? 1 : new Random().Next(2, 15);
                weapon.TuneToPrice(price, distance);
                string weaponName = distance == 1 ? WeaponNames.Random() : RangedWeaponNames.Random();
                weaponName = WeaponNarrativePrefixes.Random() + " " + weaponName;

                if (price > gradeStep)
                {
                    int gradeIndex = price / gradeStep - 1;
                    string grade = gradeIndex < WeaponGradesNames.Count 
                        ? WeaponGradesNames[gradeIndex]
                        : WeaponGradesNames.Last();
                    weaponName = grade + " " + weaponName;
                }

                weapon.Name = weaponName;

                weapons.Add(weapon);
            }

            return weapons.OrderBy(x => x.Distance).ToList();
        }
    }

}
