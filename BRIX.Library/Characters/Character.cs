﻿using BRIX.Library.Aspects;
using BRIX.Library.Extensions;
using BRIX.Utility.Extensions;
using System.Dynamic;
using System.Linq;
using System.Net;
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
        public string Backstory { get; set; }
        public string Appearance { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<CharacterProject> Projects { get; set; } = new();
        public List<CharacterAbility> Abilities { get; set; } = new();
        public List<AbilityMaterialSupport> MaterialSupport { get; set; } = new();
        public Inventory Inventory { get; set; } = new();

        private int _experience;
        public int Experience
        {
            get => _experience;
            set
            {
                int oldLevel = Level;
                _experience = value;

                if(Level != oldLevel)
                {
                    CurrentHealth = MaxHealth;
                }
            }
        }

        private int _expInHealth;
        /// <summary>
        /// Очки опыта, влитые в здоровье
        /// </summary>
        public int ExpInHealth
        {
            get => _expInHealth;
            set
            {
                int oldMaxHP = MaxHealth;
                _expInHealth = value;

                if(MaxHealth != oldMaxHP)
                {
                    CurrentHealth = MaxHealth;
                }
            }
        }

        public int Level => CharacterCalculator.GetLevelFromExp(Experience);

        public int ExpToLevelUp => CharacterCalculator.GetExpToLevelUp(Experience);
        public int AvailableExp => Experience - SpentExp;
        public int SpentExp => ExpSpentOnAbilities + ExpInHealth;
        public int ExpSpentOnAbilities => Abilities.Sum(x => x.ExpCost(this));

        public int RawHealth => Level * CharacterCalculator.HealthPerLevel;
        public int MaxHealth => RawHealth + CharacterCalculator.ExpToHealth(ExpInHealth);
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Здесь зависимость от способностей, но временно считается по простому.
        /// </summary>
        public int MaxActionPoints => 5;
        public int CurrentActionPoints { get; set; } = 5;

        public CharacterPortrait Portrait { get; set; } = new();

        public List<MaterialSupport> GetMaterialSupportForAbility(CharacterAbility ability)
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

        public List<Consumable> GetConsumablesForAbility(CharacterAbility ability)
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
        public bool GetAbilityAvailability(CharacterAbility ability)
        {
            if(!Abilities.Contains(ability))
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

            foreach(Consumable consumable in GetConsumablesForAbility(ability))
            {
                consumable.Count--;
            }

            ActionPointAspect apAspect = ability.GetAspect<ActionPointAspect>();

            if (apAspect != null)
            {
                CurrentActionPoints -= apAspect.ActionPoints;
            }
        }

        /// <summary>
        /// Заменяет материальное обеспечение в инвентаре.
        /// При изменении стоимости обеспечения произойдёт пересчёт опыта.
        /// Если персонажу не хватает опыта на такое изменение, то изменение не произойдёт, а метод вернёт False.
        /// </summary>
        public bool UpdateMaterialSupport(MaterialSupport itemToUpdate)
        {
            if(!Inventory.Items.Any(x => x.Equals(itemToUpdate)))
            {
                throw new AbilityLogicException("У персонажа не найдено соответствующее материальное обеспечение.");
            }

            Character copyOfThis = this.Copy();
            MaterialSupport existingItem = Inventory.Items.Single(x => x.Equals(itemToUpdate)) as MaterialSupport;

            if (existingItem != null)
            {
                copyOfThis?.Inventory.Swap(existingItem, itemToUpdate);
            }

            if(copyOfThis?.AvailableExp < 0)
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
                throw new AbilityLogicException("У персонажа не найдено соответствующее материальное обеспечение.");
            }

            if(!CanRemoveMaterialSupport(itemToRemove))
            {
                return false;
            }

            Inventory.Remove(itemToRemove, saveContent);
            MaterialSupport.RemoveAll(x => x.MaterialSupportId == itemToRemove.Id);

            return true;
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
            return MaterialSupport.Any(x => x.MaterialSupportId == item.Id);
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
