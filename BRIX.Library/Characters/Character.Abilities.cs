using BRIX.Library.Abilities;
using BRIX.Library.Characters.Inventory;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;

namespace BRIX.Library.Characters
{
    public partial class Character
    {
        public List<ConsumableItem> GetConsumablesForAbility(Ability ability)
        {
            List<Guid> itemsGuids = AbilityConsumables
                .Where(x => x.AbilityId == ability.Id)
                .Select(x => x.ConsumableId)
                .ToList();

            return Inventory.Items
                .Where(x => itemsGuids.Any(y => y == x.Id) && x is ConsumableItem)
                .Cast<ConsumableItem>()
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

            List<ConsumableItem> materialSupport = GetConsumablesForAbility(ability);

            foreach (ConsumableItem abilityMaterial in materialSupport)
            {
                bool matirealExistsAndAvailiable = Inventory.Items.Any(x =>
                    x is ConsumableItem existingMaterial
                    && existingMaterial.Equals(abilityMaterial)
                    && existingMaterial.IsAvailiable
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
        public void ActivateAbility(Ability ability)
        {
            if (!Abilities.Contains(ability) || !GetAbilityAvailability(ability))
            {
                return;
            }

            foreach (ConsumableItem consumable in GetConsumablesForAbility(ability))
            {
                consumable.Count--;
            }

            if (CurrentActionPoints > ability.Activation.ActionPoints)
            {
                CurrentActionPoints -= ability.Activation.ActionPoints;
            }
        }

        /// <summary>
        /// Заменяет расходник способности на переданный, с обновлёнными свойствами.
        /// При изменении стоимости обеспечения произойдёт пересчёт опыта.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт, а метод вернёт False.
        /// </summary>
        public bool UpdateConsumable(ConsumableItem itemToUpdate)
        {
            if (!Inventory.Items.Any(x => x.Equals(itemToUpdate)))
            {
                throw new Exception("Такого предмета нет в инвентаре.");
            }

            ConsumableItem existingItem = (ConsumableItem)Inventory.Items.Single(x => x.Equals(itemToUpdate));
            int dependentAbilitiesCount = ConsumableDependedAbilitiesCount(existingItem);
            int expDiff = itemToUpdate.ToExpEquivalent() * dependentAbilitiesCount
                - existingItem.ToExpEquivalent() * dependentAbilitiesCount;

            if (expDiff < 0)
            {
                return false;
            }

            Inventory.Swap(Inventory.Items.Single(x => x.Equals(itemToUpdate)), itemToUpdate);

            return true;
        }

        /// <summary>
        /// Удаляет материальное обеспечение из инвентаря и способностей, зависимые способности станут дороже.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт, а метод вернёт False.
        /// </summary>
        public bool RemoveConsumable(ConsumableItem itemToRemove, bool saveContent = false)
        {
            if (!Inventory.Items.Contains(itemToRemove))
            {
                throw new Exception("Такого предмета нет в инвентаре.");
            }

            if (!CanRemoveConsumable(itemToRemove))
            {
                return false;
            }

            Inventory.Remove(itemToRemove, saveContent);
            AbilityConsumables.RemoveAll(x => x.ConsumableId == itemToRemove.Id);

            return true;
        }

        public bool CanRemoveConsumable(ConsumableItem itemToRemove)
        {
            if(!Inventory.Items.Contains(itemToRemove))
            {
                throw new Exception("Такого предмета нет в инвентаре.");
            }

            int dependentAbilitiesCount = ConsumableDependedAbilitiesCount(itemToRemove);

            if (dependentAbilitiesCount == 0)
            {
                return true;
            }

            int expDiff = itemToRemove.ToExpEquivalent() * dependentAbilitiesCount;

            return AvailableExp > expDiff;
        }

        public int ConsumableDependedAbilitiesCount(ConsumableItem item)
        {
            return AbilityConsumables.Count(x => x.ConsumableId == item.Id);
        }
    }
}