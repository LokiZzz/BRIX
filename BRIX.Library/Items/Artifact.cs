using BRIX.Library.Abilities;
using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;

namespace BRIX.Library.Items
{
    /// <summary>
    /// Предмет типа «Артефакт»
    /// </summary>
    public class Artifact : Item
    {
        /// <summary>
        /// Урон артефакта. Бонус к урону, который наносит персонаж. По-умолчанию бонус нулевой.
        /// </summary>
        public DicePool Damage { get; set; } = new(0);

        /// <summary>
        /// Дистанция артефакта. Бонус к урону может быть применён только к тем целям, которые расположены на указанном
        /// расстоянии или дальше.
        /// </summary>
        public int Distance { get; set; } = 1;

        /// <summary>
        /// Показатель защиты артефакта. Снижает урон, который получает персонаж. По-умолчанию бонус нулевой.
        /// </summary>
        public DicePool Defense { get; set; } = new(0);

        /// <summary>
        /// Особенности артефакта. Пока персонаж носит или держит артефакт, он может пользоваться его особенностями 
        /// так, как если бы они были его способностями.
        /// </summary>
        public List<ArtifactFeature> Features { get; set; } = [];

        /// <summary>
        /// Объективная стоимость артефакта в монетах. Стоимость — мерило мощности артефакта.
        /// </summary>
        public int Price => GetPrice();

        /// <summary>
        /// Уровень артефакта. Если уровень персонажа ниже, то он не может пользоваться этим артефактом.
        /// </summary>
        public int Level => ExperienceCalculator.GetLevelFromExp(Price / _costExpMultiplier);

        private readonly int _costExpMultiplier = 5;

        private int GetPrice()
        {
            DamageEffect damage = new() { Impact = Damage };
            damage.GetAspect<TargetSelectionAspect>().NTAD.DistanceInMeters = Distance;
            int damagePrice = damage.GetExpCost() * 5;
            
            DefenseEffect defense = new() { Impact = Defense };
            defense.GetAspect<TargetSelectionAspect>().Strategy = ETargetSelectionStrategy.CharacterHimself;
            defense.GetAspect<DurationAspect>().Duration = 5; // Т.к. большинство боёв заканчиваются раньше, чем за 5 раундов.
            int defensePrice = defense.GetExpCost();

            int featurePrice = Features.Sum(x => x.ExpCost());

            return (damagePrice + defensePrice + featurePrice) * _costExpMultiplier;
        }

        public void AddFeature(ArtifactFeature feature)
        {
            Features.Add(feature);
        }

        public bool UpdateFeature(ArtifactFeature feature)
        {
            if (Features.Any(x => x.Id == feature.Id))
            {
                int index = Features.IndexOf(Features.First(x => x.Id == feature.Id));
                Features[index] = feature;

                return true;
            }

            return false;
        }

        public void RemoveFeature(ArtifactFeature feature) => Features.RemoveAll(x => x.Id == feature.Id);
    }
}
