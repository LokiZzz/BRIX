using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities
{
    public class CharacterAbilityModel : ObservableObject
    {
        public CharacterAbilityModel() : this(new CharacterAbility()) { }

        public CharacterAbilityModel(CharacterAbility ability)
        {
            InternalModel = ability;
            Activation = new(ability.Activation);
            Effects = new ObservableCollection<EffectModelBase>(
                ability.Effects.Select(EffectModelFactory.GetModel)
            );
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public Character? Character;

        public CharacterAbility InternalModel { get; }

        private AbilityActivationModel _activation = new();
        public AbilityActivationModel Activation 
        { 
            get => _activation;
            set
            {
                _activation = value;
                InternalModel.Activation = value.InternalModel;
            }
        }

        public ObservableCollection<EffectModelBase> Effects { get; set; } = [];

        public string Name
        {
            get => InternalModel.Name;
            set => SetProperty(
                InternalModel.Name, value, InternalModel, (character, name) => character.Name = name
            );
        }

        public string Description
        {
            get => InternalModel.Description;
            set => SetProperty(
                InternalModel.Description, value, InternalModel, (ability, desc) => ability.Description = desc
            );
        }

        public string ActivationDescription => InternalModel.Activation.ToLexis();

        public string StatusName
        {
            get => InternalModel.StatusName;
            set => SetProperty(
                InternalModel.StatusName, value, InternalModel, (character, status) => character.StatusName = status
            );
        }

        private int _cost;
        public int Cost
        {
            get
            {
                _cost = InternalModel.ExpCost(Character);

                return _cost;
            }
        }

        public bool ShowStatusName => InternalModel.HasStatus;

        public void AddEffect(EffectModelBase effect)
        {
            if(effect.InternalModel == null)
            {
                throw new Exception("Не инициализирована модель" + nameof(effect.InternalModel));
            }

            InternalModel.AddEffect(effect.InternalModel);
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

            InternalModel.UpdateEffect(effect.InternalModel);
            EffectModelBase effectToRemove = Effects.First(x =>
                x.InternalModel?.Number == effect.InternalModel.Number
                && x.InternalModel.GetType().Equals(effect.InternalModel.GetType())
            );
            Effects.Remove(effectToRemove);
            Effects.Add(effect);
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public void RemoveEffect(EffectModelBase effect) 
        {
            if(effect.InternalModel == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            InternalModel.RemoveEffect(effect.InternalModel);
            Effects.Remove(effect);
            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(ShowStatusName));
        }

        public int UpdateCost()
        {
            OnPropertyChanged(nameof(Cost));

            return _cost;
        }
    }
}
