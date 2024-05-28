using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Extensions;
using BRIX.Library.Mathematics;

namespace BRIX.Library.Effects
{
    public class ShieldEffect : EffectBase
    {
        public override List<Type> RequiredAspects =>
        [
            typeof(ActivationConditionsAspect)
        ];

        /// <summary>
        /// Прочность щита, исчисляемая в очках здоровья.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Можно ли что-либо видеть сквозь щит.
        /// </summary>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// Могут ли сквозь щит по желанию персонажа проходить объекты.
        /// </summary>
        public bool CanBePermeable { get; set; }

        public VolumeShape ShieldShape { get; set; } = new();

        public override int BaseExpCost()
        {
            int baseByHealth = (CharacterCalculator.HealthToExp(Health) * 0.5).Round();
            double transparentCoef = IsTransparent ? 1.1 : 1;
            double permeableCoef = CanBePermeable ? 2 : 1;
            double volumeCoef = (ShieldShape.Shape?.GetVolume() ?? 0 * 5).ToCoeficient();

            return (baseByHealth * transparentCoef * permeableCoef * volumeCoef).Round();
        }
    }
}
