using BRIX.Library.Characters;
using BRIX.Library.Extensions;

namespace BRIX.Library.Abilities
{
    public class CharacterAbility : Ability
    {
       /// <summary>
        /// Получить стоимость способности в очках опыта. 
        /// Можно передать персонажа, для которого будет рассчитана индивидуальная стоимость.
        /// Индивидуальная стоимость может происходить из того, что у персонажа будет материальное обеспечение для
        /// данной способности.
        /// </summary>
        public int ExpCost(Character? character = null)
        {
            int expCost = base.ExpCost();

            if (character != null)
            {
                IEnumerable<AbilityMaterialSupport> abilityMaterialSupport = character.MaterialSupport
                    .Where(x => x.AbilityId == Id);

                foreach (AbilityMaterialSupport item in abilityMaterialSupport)
                {
                    InventoryItem matirealSupport = character.Inventory.Items
                        .Single(x => x.Id == item.MaterialSupportId);

                    if (matirealSupport is MaterialSupport concreteItem)
                    {
                        expCost -= concreteItem.ToExpEquivalent().Round();
                    }
                }
            }

            return expCost <= 1 ? 1 : expCost;
        }
    }
}