using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace BRIX.Library.Characters
{
    public class Character
    {
        public Character()
        {
            Experience = 100;
            CurrentHealth = MaxHealth;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CharacterAbility> Abilities { get; set; } = new();
        public int Experience { get; set; }
        public string Backstory { get; set; }
        public string Appearance { get; set; }
        public List<string> Tags { get; set; } = new();
        public Inventory Inventory { get; set; } = new();

        public int Level => ExperienceCalculator.GetLevelFromExp(Experience);
        public int ExpToLevelUp => ExperienceCalculator.GetExpToLevelUp(Experience);
        public int SpentExp => Abilities.Sum(x => x.ExpCost());
        public int AvailableExp => Experience - SpentExp;

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public int MaxHealth => Level * 10;
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public int MaxActionPoints => 5;
        public int CurrentActionPoints { get; set; } = 5;

        public CharacterPortrait Portrait { get; set; } = new();

        /// <summary>
        /// Возвращает доступность способности. 
        /// Передаваемая способность должна содержаться в коллекции Abilities.
        /// </summary>
        public bool GetAbilityAvailability(CharacterAbility ability)
        {
            if(!Abilities.Contains(ability))
            {
                return false;
            }

            List<MaterialSupport> materialSupport = ability.Equipment.Cast<MaterialSupport>()
                .Union(ability.Consumables.Cast<MaterialSupport>())
                .ToList();

            foreach (MaterialSupport abilityMaterial in materialSupport)
            {
                bool matirealExistsAndAvailiable = Inventory.Items.Any(x => 
                    x is MaterialSupport existingMaterial
                    && existingMaterial.Equals(abilityMaterial)
                    && existingMaterial.IsAvailable
                );

                if(!matirealExistsAndAvailiable)
                {
                    return false;
                }
            }

            ActionPointAspect apAspect = ability.GetAspect<ActionPointAspect>();

            if (apAspect != null && apAspect.ActionPoints > CurrentActionPoints)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Активация способности персонажем — трата расходников и очков действий.
        /// </summary>
        /// <param name="ability"></param>
        public void ActivateAbility(CharacterAbility ability)
        {
            if (!Abilities.Contains(ability) || !GetAbilityAvailability(ability))
            {
                return;
            }

            foreach(Consumable consumable in ability.Consumables)
            {
                consumable.Count--;
                Inventory.Items.Single(x => x.Equals(consumable)).Count--;
            }

            ActionPointAspect apAspect = ability.GetAspect<ActionPointAspect>();

            if (apAspect != null)
            {
                CurrentActionPoints -= apAspect.ActionPoints;
            }
        }

        /// <summary>
        /// Заменяет материальное обеспечение в инвентаре и способностях.
        /// При изменении стоимости обеспечения произойдёт пересчёт опыта.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт
        /// и будет выброшено исключение.
        /// </summary>
        public void UpdateMaterialSupport(MaterialSupport itemToUpdate)
        {
            if(!Inventory.Items.Any(x => x.Equals(itemToUpdate)))
            {
                throw new AbilityLogicException("У персонажа не найдено соответствующее материальное обеспечение.");
            }

            MaterialSupport existingItem = (MaterialSupport)Inventory.Items.Single(x => x.Equals(itemToUpdate));
            int expDiff = (existingItem.ToExpEquivalent() - itemToUpdate.ToExpEquivalent()).Round();
            bool notEnoughEXP = AvailableExp < -expDiff;

            if(notEnoughEXP)
            {
                throw new NotEnoughEXPForChangesException(
                    "У персонажа не хватает опыта, чтобы компенсировать удешевление материального обеспечение."
                );
            }

            Inventory.Items.Replace(
                Inventory.Items.Single(x => x.Equals(itemToUpdate)), 
                itemToUpdate
            );

            foreach (CharacterAbility ability in Abilities)
            {
                Consumable? consumable = ability.Consumables.FirstOrDefault(x => x.Equals(itemToUpdate));

                if(consumable != null)
                {
                    ability.Consumables.Replace(consumable, itemToUpdate);
                }

                Equipment? equipment = ability.Equipment.FirstOrDefault(x => x.Equals(itemToUpdate));

                if (equipment != null)
                {
                    ability.Equipment.Replace(equipment, itemToUpdate);
                }
            }
        }

        /// <summary>
        /// Удаляет материальное обеспечение в инвентаре и способностях,
        /// зависимые способности станут дороже.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт
        /// и будет выброшено исключение.
        /// </summary>
        public void RemoveMaterialSupport(MaterialSupport itemToRemove, bool saveContent = false)
        {
            if (!Inventory.Items.Any(x => x.Equals(itemToRemove)))
            {
                throw new AbilityLogicException("У персонажа не найдено соответствующее материальное обеспечение.");
            }

            if(!CanRemoveMaterialSupport(itemToRemove))
            {
                throw new NotEnoughEXPForChangesException(
                    "У персонажа не хватает опыта, чтобы компенсировать удешевление материального обеспечение."
                );
            }

            Inventory.Remove(itemToRemove, saveContent);

            foreach (CharacterAbility ability in Abilities)
            {
                Consumable? consumable = ability.Consumables.FirstOrDefault(x => x.Equals(itemToRemove));

                if (consumable != null)
                {
                    ability.Consumables.Remove(consumable);
                }

                Equipment? equipment = ability.Equipment.FirstOrDefault(x => x.Equals(itemToRemove));

                if (equipment != null)
                {
                    ability.Equipment.Remove(equipment);
                }
            }
        }

        public bool CanRemoveMaterialSupport(MaterialSupport itemToRemove)
        {
            if(!HaveMaterialDependedAbilities(itemToRemove))
            {
                return true;
            }

            MaterialSupport existingItem = (MaterialSupport)Inventory.Items.Single(x => x.Equals(itemToRemove));
            int expDiff = itemToRemove.ToExpEquivalent().Round();
            
            return AvailableExp > expDiff;
        }

        public bool HaveMaterialDependedAbilities(MaterialSupport item)
        {
            return Abilities.Any(x => 
                x.Equipment.Any(x => x.Equals(item)) 
                || x.Consumables.Any(x => x.Equals(item))
            );
        }
    }

    public class CharacterPortrait
    {
        public string Path { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double S { get; set; }
    }
}
