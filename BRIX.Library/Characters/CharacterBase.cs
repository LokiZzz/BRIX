using BRIX.Library.Abilities;
using BRIX.Library.Effects;

namespace BRIX.Library.Characters
{
    public abstract class CharacterBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];

        public List<CharacterAbility> Abilities { get; set; } = [];
        public List<Status> Statuses { get; set; } = [];

        /// <summary>
        /// Способности, создающие статус.
        /// </summary>
        public List<CharacterAbility> StatusAbilities
        {
            get
            {
                if(Abilities.Count == 0)
                {
                    return [];
                }

                return Abilities.Where(x => x.HasStatus).ToList();
            }
        }

        public abstract int RawMaxHealth { get; }
        public int MaxHealthBonuses => GetMaxHealthBonuses();
        public int MaxHealthPenalties => GetMaxHealthPenalties(); 
        public int MaxHealth => GetMaxHealth();
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public static int MaxActionPoints => 5;
        public int CurrentActionPoints { get; set; } = 5;

        protected virtual int GetMaxHealth()
        {
            return RawMaxHealth + MaxHealthBonuses + MaxHealthPenalties;
        }

        private int GetMaxHealthBonuses()
        {
            IEnumerable<FortifyEffect> fortifyStatusEffects = Statuses
                .Where(x => x.Effects.Any(x => x is FortifyEffect))
                .SelectMany(x => x.Effects.Where(x => x is FortifyEffect))
                .Cast<FortifyEffect>();
            int fortifyBonus = fortifyStatusEffects
                .Where(x => x.Impact.Dice.Count == 0)
                .Sum(x => x.Impact.Modifier);

            return fortifyBonus;
        }

        private int GetMaxHealthPenalties()
        {
            IEnumerable<ExhaustionEffect> exhaustionStatusEffects = Statuses
                .Where(x => x.Effects.Any(x => x is ExhaustionEffect))
                .SelectMany(x => x.Effects.Where(x => x is ExhaustionEffect))
                .Cast<ExhaustionEffect>();
            int exhaustionBonus = exhaustionStatusEffects
                .Where(x => x.Impact.Dice.Count == 0)
                .Sum(x => x.Impact.Modifier);

            return -exhaustionBonus;
        }
    }
}