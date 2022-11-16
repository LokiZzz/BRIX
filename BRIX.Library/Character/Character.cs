namespace BRIX.Library.Character
{
    public class Character
    {
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
    }
}
