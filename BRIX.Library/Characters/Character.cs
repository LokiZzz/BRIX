namespace BRIX.Library.Characters
{
    public class Character
    {
        public Character()
        {
            Experience = 100;
            CurrentHealth = MaxHealth;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Ability> Abilities { get; set; } = new();
        public int Experience { get; set; }
        public string Backstory { get; set; }
        public string Appearance { get; set; }

        public int Level => ExperienceCalculator.GetLevelFromExp(Experience);
        public int ExpToLevelUp => ExperienceCalculator.GetExpToLevelUp(Experience);
        public int SpentExp => Abilities.Sum(x => x.ExpCost());
        public int AvailableExp => Experience - SpentExp;

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public int MaxHealth => Level * 10;
        public int CurrentHealth { get; set; }

        public CharacterPortrait Portrait { get; set; } = new();
    }

    public class CharacterPortrait
    {
        public string Path { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double S { get; set; }
    }
}
