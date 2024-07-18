using BRIX.Library.Abilities;

namespace BRIX.Library.Characters
{
    public partial class Character : CharacterBase
    {
        public Character()
        {
            Experience = 100;
            CurrentHealth = MaxHealth;
        }

        public string Backstory { get; set; } = string.Empty;
        public string Appearance { get; set; } = string.Empty;
        public List<CharacterProject> Projects { get; set; } = [];
        public List<AbilityMaterialSupport> MaterialSupport { get; set; } = [];
        public Inventory Inventory { get; set; } = new();

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
        public int SpentExp => GetSpentExp();
        public int ExpSpentOnAbilities => Abilities.Sum(x => x.ExpCost(this));

        public int GetSpentExp(Guid? excludeAbilityId = null)
        {
            return Abilities
                .Where(x => excludeAbilityId == null || x.Id != excludeAbilityId)
                .Sum(x => x.ExpCost(this))
                + ExpInHealth
                + Speed.GetExpCost();
        }

        public override int RawMaxHealth => Level * CharacterCalculator.HealthPerLevel;
        public int HealthFromExp => CharacterCalculator.ExpToHealth(ExpInHealth);

        public CharacterPortrait Portrait { get; set; } = new();

        public int LuckPoints { get; set; }

        protected override int GetMaxHealth()
        {
            return base.GetMaxHealth() + HealthFromExp;
        }
    }
}
