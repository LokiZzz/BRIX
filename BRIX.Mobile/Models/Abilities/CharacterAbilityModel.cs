using BRIX.Library.Abilities;
using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities
{
    public class CharacterAbilityModel : ObservableObject
    {
        public CharacterAbilityModel() : this(new Ability()) { }

        public CharacterAbilityModel(Ability ability)
        {
            Internal = ability;
            Activation = new(ability.Activation);
            InitializeEffects();
            ConcordedAspects = new ObservableCollection<AspectModelBase>(
                ability.ConcordedAspects.Select(AspectModelFactory.GetAspectModel)
            );
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public CharacterBase? Character;

        public Ability Internal { get; set; }

        private AbilityActivationModel _activation = new();
        public AbilityActivationModel Activation 
        { 
            get => _activation;
            set
            {
                _activation = value;
                Internal.Activation = value.InternalModel;
                OnPropertyChanged(nameof(ActivationDescription));
            }
        }

        private ObservableCollection<EffectModelBase> _effects = [];
        public ObservableCollection<EffectModelBase> Effects
        {
            get => _effects;
            set => SetProperty(ref _effects, value);
        }

        public ObservableCollection<AspectModelBase> ConcordedAspects { get; set; } = [];

        public string Name
        {
            get => Internal.Name;
            set => SetProperty(
                Internal.Name, value, Internal, (character, name) => character.Name = name
            );
        }

        public string Description
        {
            get => Internal.Description;
            set => SetProperty(
                Internal.Description, value, Internal, (ability, desc) => ability.Description = desc
            );
        }

        public string ActivationDescription => Internal.Activation.ToLexis();

        public string StatusName
        {
            get => Internal.StatusName;
            set => SetProperty(
                Internal.StatusName, value, Internal, (character, status) => character.StatusName = status
            );
        }

        public int Cost => Internal.ExpCost();

        public bool ShowStatusName => Internal.HasStatus;

        public void AddEffect(EffectModelBase effect)
        {
            if(effect.InternalModel == null)
            {
                throw new Exception("Не инициализирована модель" + nameof(effect.InternalModel));
            }

            Internal.AddEffect(effect.InternalModel);
            Effects.Add(effect);
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public void UpdateEffect(EffectModelBase effect)
        {
            if (effect.InternalModel == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            Internal.UpdateEffect(effect.InternalModel);
            int index = Effects.IndexOf(Effects.First(x => x.InternalModel?.Id == effect.InternalModel.Id));
            Effects[index] = effect;
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public void RemoveEffect(EffectModelBase effect) 
        {
            if(effect.InternalModel == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            Internal.RemoveEffect(effect.InternalModel);
            Effects.Remove(effect);
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(ShowStatusName));
        }
        
        /// <summary>
        /// Обновляет согласованный аспект в способности, используется по возвращении на
        /// страницу способности после редактирования аспекта (и для соединения способности
        /// с редактируемым аспектом на странице редактирования аспекта).
        /// </summary>
        public void UpdateConcordedAspect(AspectModelBase aspectModel)
        {
            Internal.UpdateConcordedAspect(aspectModel.InternalModel);

            int index = ConcordedAspects.IndexOf(
                ConcordedAspects.First(x => x.InternalModel.GetType().Equals(aspectModel.InternalModel.GetType()))
            );
            ConcordedAspects[index] = aspectModel;

            InitializeEffects();

            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public void Concord(AspectModelBase aspect)
        {
            ConcordedAspects.Add(aspect);
            Internal.Concord(aspect.InternalModel);
            InitializeEffects();

            OnPropertyChanged(nameof(Cost));
        }

        public void Discord(AspectModelBase aspect)
        {
            AspectModelBase aspectToRemove = ConcordedAspects.First(x =>
                x.InternalModel.GetType().Equals(aspect.InternalModel.GetType())
            );
            ConcordedAspects.Remove(aspectToRemove);
            Internal.Discord(aspect.InternalModel.GetType());
            InitializeEffects();

            OnPropertyChanged(nameof(Cost));
        }

        /// <summary>
        /// Используется при возвращении на страницу способности после редактирования эффекта.
        /// Происходит проверка на предмет согласованных и рассогласованных эффектов и способность
        /// синхронизируется по согласованности с настройками в эффекте.
        /// </summary>
        public void UpdateConcordanceByEffect(EffectModelBase effect)
        {
            foreach (AspectModelBase aspect in effect.Aspects)
            {
                AspectModelBase? existingConcordedAspect = ConcordedAspects.FirstOrDefault(x =>
                    x.InternalModel.GetType().Equals(aspect.InternalModel.GetType())
                );

                if (aspect.IsConcorded && existingConcordedAspect == null)
                {
                    Concord(aspect);
                }
                else if(!aspect.IsConcorded && existingConcordedAspect != null)
                {
                    Discord(aspect);
                }
            }
        }

        public void UpdateCost() => OnPropertyChanged(nameof(Cost));

        protected void InitializeEffects()
        {
            Effects = new ObservableCollection<EffectModelBase>(
                Internal.Effects.Select(EffectModelFactory.GetModel)
            );
        }
    }
}
