using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Library.Characters;
using System.Collections.ObjectModel;
using BRIX.Mobile.Models.Abilities;
using System.Collections.Specialized;
using BRIX.Library.Abilities;

namespace BRIX.Mobile.Models.Characters
{
    public partial class CharacterModel : ObservableObject
    {
        public CharacterModel() : this(new Character()) { }

        public CharacterModel(Character character)
        {
            InternalModel = character;
            Abilities = new (character.Abilities.Select(x => 
                new CharacterAbilityModel(x) { Character = character })
            );
            Tags = new (character.Tags.Select(x => new CharacterTagVM { Text = x }));
            Projects = new(character.Projects.Select(x => new CharacterProjectVM(x)));
            Statuses = new(character.Statuses.Select(x => new StatusItemVM(x)));
            OnPropertyChanged(nameof(ShowStatuses));
        }

        public Character InternalModel { get; }

        private ObservableCollection<CharacterAbilityModel> _abilities = [];
        public ObservableCollection<CharacterAbilityModel> Abilities
        {
            get => _abilities;
            set => SetProperty(ref _abilities, value);
        }

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

        public int LuckPoints
        {
            get => InternalModel.LuckPoints;
            set => SetProperty(InternalModel.LuckPoints, value, InternalModel, (character, luck) => character.LuckPoints = luck);
        }

        public double PortraitX
        {
            get => InternalModel.Portrait.X;
            set => SetProperty(InternalModel.Portrait.X, value, InternalModel, (character, x) => character.Portrait.X = x);
        }

        public double PortraitY
        {
            get => InternalModel.Portrait.Y;
            set => SetProperty(InternalModel.Portrait.Y, value, InternalModel, (character, y) => character.Portrait.Y = y);
        }

        public double PortraitS
        {
            get => InternalModel.Portrait.S;
            set => SetProperty(InternalModel.Portrait.S, value, InternalModel, (character, s) => character.Portrait.S = s);
        }

        public ImageSource? PortraitImage
        {
            get
            {
                if(!string.IsNullOrEmpty(InternalModel.Portrait.Path))
                {
                    return ImageSource.FromFile(InternalModel.Portrait.Path);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool ShowImagePlaceholder => PortraitImage == null;

        public int MaxHealth => InternalModel.MaxHealth;
        public int RawMaxHealth => InternalModel.RawMaxHealth;
        public int HealthFromExp => InternalModel.HealthFromExp;
        public int MaxHealthPenalties => -InternalModel.MaxHealthPenalties;
        public int MaxHealthBonuses => InternalModel.MaxHealthBonuses;

        public bool ShowHealthFormula => MaxHealthPenalties != 0 || MaxHealthBonuses != 0;

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
                UpdateHealth();
                UpdateExp();
            }
        }

        public void UpdateHealth()
        {
            if(CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            OnPropertyChanged(nameof(CurrentHealth));
            OnPropertyChanged(nameof(MaxHealth));
            OnPropertyChanged(nameof(HealthPercent));
            OnPropertyChanged(nameof(HealthState));
            OnPropertyChanged(nameof(RawMaxHealth));
            OnPropertyChanged(nameof(HealthFromExp));
            OnPropertyChanged(nameof(MaxHealthBonuses));
            OnPropertyChanged(nameof(MaxHealthPenalties));
            OnPropertyChanged(nameof(ShowHealthFormula));
        }

        public void UpdateExp()
        {
            OnPropertyChanged(nameof(Level));
            OnPropertyChanged(nameof(ExperienceForNextLevel));
            OnPropertyChanged(nameof(LevelUpProgress));
            OnPropertyChanged(nameof(SpentExperience));
            OnPropertyChanged(nameof(FreeExperience));
        }

        public int ExperienceForNextLevel => CharacterCalculator.GetExpForLevel(Level + 1);

        public double LevelUpProgress
        {
            get
            {
                int absProgress = Experience - CharacterCalculator.GetExpForLevel(Level);

                return absProgress / (double)ExperienceForNextLevel;
            }
        }

        public int SpentExperience => InternalModel.SpentExp;

        public int FreeExperience => Experience - InternalModel.SpentExp;

        public void AddAbility(CharacterAbilityModel ability)
        {
            InternalModel.Abilities.Add(ability.InternalModel);
            Abilities.Add(ability);
        }

        public void RemoveAbility(Guid abilityGuid)
        {
            InternalModel.Abilities.RemoveAll(x => x.Id == abilityGuid);
            Abilities.Remove(Abilities.First(x => x.InternalModel.Id == abilityGuid));
        }

        /// <summary>
        /// Заменяет переданной способностью другую, с таким же Guid-ом
        /// </summary>
        public void UpdateAbility(CharacterAbilityModel ability)
        {
            int indexOfOldAbility = Abilities.IndexOf(
                Abilities.First(x => x.InternalModel.Id == ability.InternalModel.Id)
            );
            Abilities[indexOfOldAbility] = ability;

            indexOfOldAbility = InternalModel.Abilities.IndexOf(
                InternalModel.Abilities.First(x => x.Id == ability.InternalModel.Id)
            );
            InternalModel.Abilities[indexOfOldAbility] = ability.InternalModel;
        }

        public ObservableCollection<CharacterTagVM> Tags { get; set; }

        public void AddTag(CharacterTagVM tag)
        {
            Tags.Add(tag);
            InternalModel.Tags.Add(tag.Text);
        }

        public void RemoveTag(CharacterTagVM tag)
        {
            Tags.Remove(tag);
            InternalModel.Tags.Remove(InternalModel.Tags.Single(x => x == tag.Text));
        }

        public ObservableCollection<CharacterProjectVM> Projects { get; set; }

        public void AddProject(CharacterProjectVM project)
        {
            Projects.Add(project);
            InternalModel.Projects.Add(new CharacterProject { 
                Name = project.Name,
                Description = project.Description,
                Steps = project.Steps,
                CurrentStep = project.CurrentStep
            });
        }

        public void RemoveProject(CharacterProjectVM project)
        {
            Projects.Remove(project);
            InternalModel.Projects.Remove(InternalModel.Projects.Single(x => x.Name == project.Name));
        }

        public ObservableCollection<StatusItemVM> Statuses { get; set; }

        public bool ShowStatuses => Statuses?.Any() == true;

        public void RemoveStatus(StatusItemVM status)
        {
            Statuses.Remove(status);
            InternalModel.Statuses.Remove(status.Internal);
            OnPropertyChanged(nameof(ShowStatuses));
        }

        public void AddStatus(StatusItemVM status)
        {
            Statuses.Add(status);
            InternalModel.Statuses.Add(status.Internal);
            OnPropertyChanged(nameof(ShowStatuses));
        }

        public void ReplaceStatus(StatusItemVM status)
        {
            if (Statuses.Any(x => x.Internal.Equals(status.Internal)))
            {
                StatusItemVM existingStatus = Statuses.Single(x => x.Internal.Equals(status.Internal));
                Statuses[Statuses.IndexOf(existingStatus)] = status;
                InternalModel.Statuses[InternalModel.Statuses.IndexOf(existingStatus.Internal)] = status.Internal;
            }
        }
    }
}
