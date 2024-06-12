namespace BRIX.Library.Characters
{
    public class NPC : CharacterBase
    {
        public int Health { get; set; }

        public override int RawMaxHealth => Health;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Мощность NPC, выражается в очках опыта.
        /// </summary>
        public int Power => GetNPCPower();

        /// <summary>
        /// Исходя из стоимости способностей вычисляется мощность NPC, в соответствии с этим числом расчитывается
        /// ожидаемое кол-во здоровья, как если бы NPC был персонажем. Разница между ожидаемым и реальным кол-вом
        /// здоровья переводится в стоимость, выраженную в очках опыта и либо прибавляется, либо вычитается из стоимости
        /// способностей NPC.
        /// </summary>
        private int GetNPCPower()
        {
            int powerByAbilities = Abilities.Sum(x => x.ExpCost());
            int expectedHealth = CharacterCalculator.GetLevelFromExp(powerByAbilities) 
                * CharacterCalculator.HealthPerLevel;
            int healthDiff = Math.Abs(Health - expectedHealth);
            int expDiff = CharacterCalculator.HealthToExp(healthDiff);

            return expectedHealth > Health ? powerByAbilities + expDiff : powerByAbilities - expDiff;
        }
    }
}