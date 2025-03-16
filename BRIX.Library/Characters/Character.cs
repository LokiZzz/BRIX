using BRIX.Library.Abilities;
using BRIX.Library.Items;

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

        public List<string> MarksOfFate { get; set; } = [];

        public int LuckPoints { get; set; }

        public List<CharacterProject> Projects { get; set; } = [];

        public Inventory Inventory { get; set; } = new();

        public int Level => ExperienceCalculator.GetLevelFromExp(Experience);

        public int Experience { get; set; }

        public int ExpInHealth { get; set; }

        public int ExpInAbilities => Abilities.Sum(x => x.ExpCost());

        public int SpentExp => GetSpentExp();

        public int AvailableExp => Experience - SpentExp;

        public int ExpToLevelUp => ExperienceCalculator.GetExpToLevelUp(Experience);

        public override int RawMaxHealth => Level * ExperienceCalculator.HealthPerLevel;

        public int HealthFromExp => ExperienceCalculator.ExpToHealth(ExpInHealth);

        protected override int GetMaxHealth()
        {
            return base.GetMaxHealth() + HealthFromExp;
        }

        public int GetSpentExp(Guid? excludeAbilityId = null)
        {
            return Abilities
                .Where(x => excludeAbilityId == null || x.Id != excludeAbilityId)
                .Sum(x => x.ExpCost())
                + ExpInHealth
                + Speed.GetExpCost();
        }
    }
}
