namespace BRIX.Library.Characters
{
    public class NPC : CharacterBase
    {
        public NPC() 
        {
            Id = Id == Guid.Empty ? Guid.NewGuid() : Id;
        }

        public int Health { get; set; } = 10;

        public override int RawMaxHealth => Health;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Мощность NPC, выражается в очках опыта.
        /// </summary>
        public int Power => GetNPCPower();

        private int GetNPCPower()
        {
            int powerByAbilities = Abilities.Sum(x => x.ExpCost());
            int powerByHealth = CharacterCalculator.HealthToExp(Health);

            return (powerByAbilities + powerByHealth) / 2;
        }
    }
}