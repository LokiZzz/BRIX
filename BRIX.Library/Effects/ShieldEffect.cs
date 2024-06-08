using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    /// <summary>
    /// Создание щита, который имеет собственные очки здоровья (прочность).
    /// Форма и размер щита определяется аспектом «Зона действия»
    /// </summary>
    public class ShieldEffect : EffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(AOEAspect), typeof(ActivationConditionsAspect), typeof(DurationAspect)
        ];

        /// <summary>
        /// Прочность щита, исчисляемая в очках здоровья.
        /// </summary>
        public int Durability { get; set; } = 10;

        /// <summary>
        /// Можно ли что-либо видеть сквозь щит.
        /// </summary>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// Могут ли сквозь щит по желанию персонажа проходить объекты.
        /// </summary>
        public bool CanBePermeable { get; set; }

        public override int BaseExpCost()
        {
            int baseByHealth = (CharacterCalculator.HealthToExp(Durability) * 0.5).Round();
            double transparentCoef = IsTransparent ? 1.1 : 1;
            double permeableCoef = CanBePermeable ? 2 : 1;

            return (baseByHealth * transparentCoef * permeableCoef).Round();
        }
    }
}
