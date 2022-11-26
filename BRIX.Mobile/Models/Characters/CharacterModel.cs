using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Library.Characters;
using System.ComponentModel;

namespace BRIX.Mobile.Models.Characters
{
    public class CharacterModel : ObservableObject
    {
        public CharacterModel() : this(new Character()) { }

        public CharacterModel(Character character) => InternalModel = character;

        public Character InternalModel { get; }

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
                OnPropertyChanged(nameof(CurrentHealth));
            }
        }

        public double HealthPercent => CurrentHealth / (double)MaxHealth;

        public int Level => InternalModel.Level;

        public int Experience
        {
            get => InternalModel.Experience;
            set
            {
                SetProperty(InternalModel.Experience, value, InternalModel, (character, exp) => character.Experience = exp);
                OnPropertyChanged(nameof(MaxHealth));
                OnPropertyChanged(nameof(HealthPercent));
                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(ExperienceToLevelUp));
                OnPropertyChanged(nameof(LevelUpProgress));
            }
        }

        public int ExperienceToLevelUp => ExperienceCalculator.GetExpToLevelUp(Experience);

        public double LevelUpProgress
        {
            get
            {
                int absProgress = Experience - ExperienceCalculator.GetExpForLevel(Level);

                return absProgress / (double)ExperienceToLevelUp;
            }
        }

        public int SpentExperience => InternalModel.SpentExp;
    }
}
