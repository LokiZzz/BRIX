using BRIX.Library.Abilities;
using BRIX.Library.Extensions;
using BRIX.Library.Items;
using BRIX.Utility.Extensions;

namespace BRIX.Library.Characters
{
    public partial class Character
    {
        /// <summary>
        /// Возвращает доступность способности. 
        /// Передаваемая способность должна содержаться в коллекции Abilities.
        /// </summary>
        public bool GetAbilityAvailability(Ability ability)
        {
            if (!Abilities.Contains(ability))
            {
                return false;
            }

            return ability.Activation.ActionPoints <= CurrentActionPoints;
        }

        /// <summary>
        /// Активация способности персонажем — трата расходников и очков действий.
        /// </summary>
        public void ActivateAbility(Ability ability)
        {
            if (!Abilities.Contains(ability) || !GetAbilityAvailability(ability))
            {
                return;
            }

            if (CurrentActionPoints > ability.Activation.ActionPoints)
            {
                CurrentActionPoints -= ability.Activation.ActionPoints;
            }
        }
    }
}