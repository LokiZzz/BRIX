using BRIX.Library.Abilities;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;

namespace BRIX.Library.Characters
{
    public partial class Character
    {
        public List<MaterialSupport> GetMaterialSupportForAbility(Ability ability)
        {
            List<Guid> itemsGuids = MaterialSupport
                .Where(x => x.AbilityId == ability.Id)
                .Select(x => x.MaterialSupportId)
                .ToList();

            return Inventory.Items
                .Where(x => itemsGuids.Any(y => y == x.Id))
                .Cast<MaterialSupport>()
                .ToList();
        }

        public List<Consumable> GetConsumablesForAbility(Ability ability)
        {
            List<Guid> itemsGuids = MaterialSupport
                .Where(x => x.AbilityId == ability.Id)
                .Select(x => x.MaterialSupportId)
                .ToList();

            return Inventory.Items
                .Where(x => itemsGuids.Any(y => y == x.Id) && x is Consumable)
                .Cast<Consumable>()
                .ToList();
        }

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

            List<MaterialSupport> materialSupport = GetMaterialSupportForAbility(ability);

            foreach (MaterialSupport abilityMaterial in materialSupport)
            {
                bool matirealExistsAndAvailiable = Inventory.Items.Any(x =>
                    x is MaterialSupport existingMaterial
                    && existingMaterial.Equals(abilityMaterial)
                    && existingMaterial.IsAvailable
                );

                if (!matirealExistsAndAvailiable)
                {
                    return false;
                }
            }

            if (ability.Activation.ActionPoints > CurrentActionPoints)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Активация способности персонажем — трата расходников и очков действий.
        /// </summary>
        /// <param name="ability"></param>
        public void ActivateAbility(Ability ability)
        {
            if (!Abilities.Contains(ability) || !GetAbilityAvailability(ability))
            {
                return;
            }

            foreach (Consumable consumable in GetConsumablesForAbility(ability))
            {
                consumable.Count--;
            }

            if (ability.Activation.Strategy == EActivationStrategy.ActionPoints)
            {
                if (CurrentActionPoints > ability.Activation.ActionPoints)
                {
                    CurrentActionPoints -= ability.Activation.ActionPoints;
                }
            }
        }

        /// <summary>
        /// Заменяет материальное обеспечение в инвентаре.
        /// При изменении стоимости обеспечения произойдёт пересчёт опыта.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт, а метод вернёт False.
        /// </summary>
        public bool UpdateMaterialSupport(MaterialSupport itemToUpdate)
        {
            if (!Inventory.Items.Any(x => x.Equals(itemToUpdate)))
            {
                throw new Exception("У персонажа не найдено соответствующее материальное обеспечение.");
            }

            Character? copyOfThis = this.Copy();

            if (Inventory.Items.Single(x => x.Equals(itemToUpdate)) is MaterialSupport existingItem)
            {
                copyOfThis?.Inventory.Swap(existingItem, itemToUpdate);
            }

            if (copyOfThis?.AvailableExp < 0)
            {
                return false;
            }

            Inventory.Swap(Inventory.Items.Single(x => x.Equals(itemToUpdate)), itemToUpdate);

            return true;
        }

        /// <summary>
        /// Удаляет материальное обеспечение в инвентаре и способностях, зависимые способности станут дороже.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт, а метод вернёт False.
        /// </summary>
        public bool RemoveMaterialSupport(MaterialSupport itemToRemove, bool saveContent = false)
        {
            if (!Inventory.Items.Any(x => x.Equals(itemToRemove)))
            {
                throw new Exception("У персонажа не найдено соответствующее материальное обеспечение.");
            }

            if (!CanRemoveMaterialSupport(itemToRemove))
            {
                return false;
            }

            Inventory.Remove(itemToRemove, saveContent);
            MaterialSupport.RemoveAll(x => x.MaterialSupportId == itemToRemove.Id);

            return true;
        }

        public bool CanRemoveMaterialSupport(MaterialSupport itemToRemove)
        {
            if (!HaveMaterialDependedAbilities(itemToRemove))
            {
                return true;
            }

            MaterialSupport existingItem = (MaterialSupport)Inventory.Items.Single(x => x.Equals(itemToRemove));
            int expDiff = itemToRemove.ToExpEquivalent().Round();

            return AvailableExp > expDiff;
        }

        public bool HaveMaterialDependedAbilities(MaterialSupport item)
        {
            return MaterialSupport.Any(x => x.MaterialSupportId == item.Id);
        }
    }
}