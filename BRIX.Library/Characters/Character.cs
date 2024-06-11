using BRIX.Library.Abilities;
using BRIX.Library.Effects;

namespace BRIX.Library.Characters
{
    public partial class Character
    {
        public Character()
        {
            Experience = 100;
            CurrentHealth = MaxHealth;
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Backstory { get; set; } = string.Empty;
        public string Appearance { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
        public List<CharacterProject> Projects { get; set; } = [];
        public List<CharacterAbility> Abilities { get; set; } = [];
        public List<AbilityMaterialSupport> MaterialSupport { get; set; } = [];
        public Inventory Inventory { get; set; } = new();
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

        private int _experience;
        public int Experience
        {
            get => _experience;
            set
            {
                int oldLevel = Level;
                _experience = value;

                if(Level != oldLevel)
                {
                    CurrentHealth = MaxHealth;
                }
            }
        }

        private int _expInHealth;
        /// <summary>
        /// Очки опыта, влитые в здоровье
        /// </summary>
        public int ExpInHealth
        {
            get => _expInHealth;
            set
            {
                int oldMaxHP = MaxHealth;
                _expInHealth = value;

                if(MaxHealth != oldMaxHP)
                {
                    CurrentHealth = MaxHealth;
                }
            }
        }

        public int Level => CharacterCalculator.GetLevelFromExp(Experience);

        public int ExpToLevelUp => CharacterCalculator.GetExpToLevelUp(Experience);
        public int AvailableExp => Experience - SpentExp;
        public int SpentExp => ExpSpentOnAbilities + ExpInHealth;
        public int ExpSpentOnAbilities => Abilities.Sum(x => x.ExpCost(this));

        public int RawMaxHealth => Level * CharacterCalculator.HealthPerLevel;
        public int MaxHealthBonuses => GetMaxHealthBonuses();
        public int MaxHealthPenalties => GetMaxHealthPenalties(); 
        public int HealthFromExp => CharacterCalculator.ExpToHealth(ExpInHealth);
        public int MaxHealth => RawMaxHealth + HealthFromExp + MaxHealthBonuses + MaxHealthPenalties;
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public static int MaxActionPoints => 5;
        public int CurrentActionPoints { get; set; } = 5;

        public CharacterPortrait Portrait { get; set; } = new();

        public int LuckPoints { get; set; }

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
