using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using System.ComponentModel.Design;

namespace BRIX.Library.Effects
{
    public class SummonCreatureEffect : EffectBase
    {
        public override bool IsPositive => true;
        
        public override List<Type> RequiredAspects =>
        [
            typeof(AOEAspect), typeof(DurationAspect), typeof(ActivationConditionsAspect)
        ];

        public List<CreaturesGroup> Creatures { get; set; } = [];

        public override int BaseExpCost()
        {
            return Creatures.Sum(x =>
                // Ранжирование: 1 => 2; 2 => 4; 3 => 9; 4 => 12; 5 => 15; n => 3n; ...
                Math.Max(x.Count, 2) * Math.Min(x.Count, 3) 
                * GetPowerForSummon(x.Creature)
            );
        }

        private static int GetPowerForSummon(NPC creature)
        {
            int abilityPower = creature.Abilities.Sum(x => x.ExpCost());
            int healthPower = CharacterCalculator.HealthToExp(creature.SetHealth);
            int speedPower = creature.Speed.GetExpCost();
            // Защита от призыва стеклянной пушки. Снижается важность здоровья и скорости в расчёте.
            // Вместо просто среднего арифметического используется среднее с коэффициентами влияния.
            // Мощность способностей в десять раз более важная, чем здоровье или скорость существа.
            // Если игрок начинает наоборот, накручивать много здоровья существу, то здоровье снова 
            // начинает иметь высокий вес в формуле.
            int importanceCoef = 10;
            double initialPower = abilityPower > healthPower
                ? (speedPower + healthPower + abilityPower * importanceCoef) / (importanceCoef + 1)
                : (speedPower + healthPower + abilityPower) / 2;

            // Уравновешивание стоимости в соответствии с синергией уязвимостей.
            initialPower *= creature.Abilities.Any(x => x.Effects.Any(y => y is VulnerabilityEffect)) ? 3 : 1;

            // Повышение стоимости в соответствии с эквивалентом эффективности между простым NPC и персонажем с
            // призванным существом.
            initialPower *= 1.5;

            return Math.Max(initialPower.Round(), 50);
        }
    }

    public class CreaturesGroup
    {
        public NPC Creature { get; set; } = new();

        public int Count { get; set; }
    }
}
