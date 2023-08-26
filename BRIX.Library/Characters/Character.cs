using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;

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
        public string Name { get; set; }
        public string Backstory { get; set; }
        public string Appearance { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<CharacterProject> Projects { get; set; } = new();
        public List<CharacterAbility> Abilities { get; set; } = new();
        public List<AbilityMaterialSupport> MaterialSupport { get; set; } = new();
        public Inventory Inventory { get; set; } = new();
        public List<Status> Statuses { get; set; } = new();

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

        public int RawHealth => Level * CharacterCalculator.HealthPerLevel;
        public int MaxHealth => RawHealth + CharacterCalculator.ExpToHealth(ExpInHealth);
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public int MaxActionPoints => 5;
        public int CurrentActionPoints { get; set; } = 5;

        public CharacterPortrait Portrait { get; set; } = new();
    }
}
