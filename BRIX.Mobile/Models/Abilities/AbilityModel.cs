﻿using BRIX.Library;
using BRIX.Mobile.Models.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.Models.Abilities
{
    public class AbilityModel : ObservableObject
    {
        public AbilityModel() : this(new Ability()) { }

        public AbilityModel(Ability ability)
        {
            InternalModel = ability;
            Effects = new ObservableCollection<EffectModelBase>(
                ability.Effects.Select(EffectModelFactory.GetModel)
            );
        }

        public Ability InternalModel { get; }

        public ObservableCollection<EffectModelBase> Effects { get; set; } = new ObservableCollection<EffectModelBase>();

        public string Name
        {
            get => InternalModel.Name;
            set => SetProperty(InternalModel.Name, value, InternalModel, (character, name) => character.Name = name);
        }

        public string Description
        {
            get => InternalModel.Description;
            set => SetProperty(InternalModel.Description, value, InternalModel, (ability, desc) => ability.Description = desc);
        }

        public int Cost => InternalModel.ExpCost();

        public void AddEffect(EffectModelBase effect)
        {
            InternalModel.AddEffect(effect.InternalModel);
            Effects.Add(effect);
            OnPropertyChanged(nameof(Cost));
        }

        public void UpdateEffect(EffectModelBase effect)
        {
            InternalModel.UpdateEffect(effect.InternalModel);
            EffectModelBase effectToRemove = Effects.First(x =>
                x.InternalModel.Number == effect.InternalModel.Number
                && x.InternalModel.GetType().Equals(effect.InternalModel.GetType())
            );
            Effects.Remove(effectToRemove);
            Effects.Add(effect);
            OnPropertyChanged(nameof(Cost));
        }

        public void RemoveEffect(EffectModelBase effect) 
        {
            InternalModel.RemoveEffect(effect.InternalModel);
            Effects.Remove(effect);
            OnPropertyChanged(nameof(Cost));
        }

        public void UpdateCost()
        {
            OnPropertyChanged(nameof(Cost));
        }
    }
}
