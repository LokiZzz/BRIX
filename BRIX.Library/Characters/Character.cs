using BRIX.Library.Aspects;
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
        public List<Ability> Abilities { get; set; } = new();
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
        /// Возвращает доступность способности. Передаваемая способность должна 
        /// содержаться в коллекции Abilities.
        /// </summary>
        public bool GetAbilityAvailability(Ability ability)
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

        public void ActivateAbility(Ability ability)
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

        public void UpdateInventoryItem(InventoryItem itemToUpdate)
        {
        }

        //public void RemoveInventoryItem(InventoryItem itemToRemove, bool saveContent = false)
        //{
        //    Inventory.Remove(itemToRemove, saveContent);
            
        //    foreach(Ability ability in Abilities)
        //    {
        //        Consumable? consumable = ability.Consumables.FirstOrDefault(x => x.Equals(itemToRemove));

        //        if (consumable != null)
        //        {

        //        }
        //    }
        //}
    }

    public class CharacterPortrait
    {
        public string Path { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double S { get; set; }
    }
}
