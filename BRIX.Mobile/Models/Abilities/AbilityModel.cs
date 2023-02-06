using BRIX.Library;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities
{
    public class AbilityModel : ObservableObject
    {
        public AbilityModel() : this(new Ability()) { }

        public AbilityModel(Ability ability) => InternalModel = ability;

        public Ability InternalModel { get; }

        public ObservableCollection<EffectModel> Effects { get; set; } = new ObservableCollection<EffectModel>();

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

        public void AddEffect(EffectModel effect)
        {
            InternalModel.AddEffect(effect.InternalModel);
            Effects.Add(effect);
            OnPropertyChanged(nameof(Cost));
        }

        public void UpdateEffect(EffectModel effect)
        {
            InternalModel.UpdateEffect(effect.InternalModel);
            EffectModel effectToRemove = Effects.First(x =>
                x.InternalModel.Number == effect.InternalModel.Number
                && x.InternalModel.GetType().Equals(effect.GetType())
            );
            Effects.Remove(effectToRemove);
            Effects.Add(effect);
            OnPropertyChanged(nameof(Cost));
        }

        public void RemoveEffect(EffectModel effect) 
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
