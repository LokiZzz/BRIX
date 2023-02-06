using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Library.Characters;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BRIX.Mobile.Models.Abilities;

namespace BRIX.Mobile.Models.Characters
{
    public class CharacterModel : ObservableObject
    {
        public CharacterModel() : this(new Character()) { }

        public CharacterModel(Character character)
        {
            InternalModel = character;
            Abilities = new ObservableCollection<AbilityModel>(character.Abilities.Select(x => new AbilityModel(x)));
        }

        public Character InternalModel { get; }

        public ObservableCollection<AbilityModel> Abilities = new ObservableCollection<AbilityModel>();

        public Guid Id
        {
            get => InternalModel.Id;
            set => SetProperty(InternalModel.Id, value, InternalModel, (character, id) => character.Id = id);
        }

        public string Name
        {
            get => InternalModel.Name;
            set => SetProperty(InternalModel.Name, value, InternalModel, (character, name) => character.Name = name);
        }

        public string Backstory
        {
            get => InternalModel.Backstory;
            set => SetProperty(InternalModel.Backstory, value, InternalModel, (character, backstory) => character.Backstory = backstory);
        }

        public string Appearance
        {
            get => InternalModel.Appearance;
            set => SetProperty(InternalModel.Appearance, value, InternalModel, (character, appearance) => character.Appearance = appearance);
        }

        public int MaxHealth => InternalModel.MaxHealth;

        public int CurrentHealth
        {
            get => InternalModel.CurrentHealth;
            set
            {
                SetProperty(InternalModel.CurrentHealth, value, InternalModel, (character, health) => character.CurrentHealth = health);
                OnPropertyChanged(nameof(HealthPercent));
                OnPropertyChanged(nameof(HealthState));
                OnPropertyChanged(nameof(IsOnVergeOfLifeAndDeath));
            }
        }

        public double HealthPercent => CurrentHealth / (double)MaxHealth;

        public EHealthState HealthState
        {
            get
            {
                switch (HealthPercent)
                {
                    case > .50:
                        return EHealthState.Fine;
                    case < .50 and >= .25:
                        return EHealthState.Bad;
                    case < .25:
                        return EHealthState.Critical;
                    default:
                        return EHealthState.Fine;
                }
            }
        }

        public bool IsOnVergeOfLifeAndDeath => CurrentHealth == 0;

        public int Level => InternalModel.Level;

        public int Experience
        {
            get => InternalModel.Experience;
            set
            {
                SetProperty(InternalModel.Experience, value, InternalModel, (character, exp) => character.Experience = exp);
                OnPropertyChanged(nameof(MaxHealth));
                OnPropertyChanged(nameof(HealthPercent));
                OnPropertyChanged(nameof(HealthState));
                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(ExperienceForNextLevel));
                OnPropertyChanged(nameof(LevelUpProgress));
                OnPropertyChanged(nameof(SpentExperience));
                OnPropertyChanged(nameof(FreeExperience));
            }
        }

        public int ExperienceForNextLevel => ExperienceCalculator.GetExpForLevel(Level + 1);

        public double LevelUpProgress
        {
            get
            {
                int absProgress = Experience - ExperienceCalculator.GetExpForLevel(Level);

                return absProgress / (double)ExperienceForNextLevel;
            }
        }

        public int SpentExperience => InternalModel.SpentExp;

        public int FreeExperience => Experience - InternalModel.SpentExp;

        public void AddAbility(AbilityModel ability)
        {
            InternalModel.Abilities.Add(ability.InternalModel);
            Abilities.Add(ability);
        }

        /// <summary>
        /// Заменяет переданной способностью другую, с таким же Guid-ом
        /// </summary>
        public void RemoveAbility(Guid abilityGuid)
        {
            InternalModel.Abilities.RemoveAll(x => x.Guid == abilityGuid);
            Abilities.Remove(Abilities.First(x => x.InternalModel.Guid == abilityGuid));
        }

        /// <summary>
        /// Заменяет переданной способностью другую, с таким же Guid-ом
        /// </summary>
        public void UpdateAbility(AbilityModel ability)
        {
            RemoveAbility(ability.InternalModel.Guid);
            AddAbility(ability);
        }
    }

    public enum EHealthState
    {
        Fine = 0,
        Bad = 1,
        Critical
    }
}
