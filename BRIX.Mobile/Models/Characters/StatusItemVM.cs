using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Library.Ability;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.ViewModel.Abilities.Effects;
using BRIX.Library.Effects;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using BRIX.Mobile.Models.Abilities.Effects;

namespace BRIX.Mobile.Models.Characters
{
    public partial class StatusItemVM(Status status) : ObservableObject
    {
        public Status Internal { get; set; } = status;

        public ObservableCollection<EffectModelBase> Effects { get; set; } = new (
            status.Effects.Select(EffectModelFactory.GetModel)
        );

        public string Name
        {
            get => Internal.Name;
            set => SetProperty(Internal.Name, value, Internal, (status, name) => status.Name = name);
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public string RoundsLeft 
        {
            get
            {
                return Internal.GetHighestTimeUnit() switch
                {
                    Library.Enums.ETimeUnit.Round => 
                        string.Format(Localization.RoundsCountFormat, Internal.DurationLeft),
                    Library.Enums.ETimeUnit.Minute => 
                        string.Format(Localization.MinutesCountFormat, Internal.DurationLeft),
                    Library.Enums.ETimeUnit.Hour => 
                        string.Format(Localization.HoursCountFormat, Internal.DurationLeft),
                    Library.Enums.ETimeUnit.Day => 
                        string.Format(Localization.DaysCountFormat, Internal.DurationLeft),
                    Library.Enums.ETimeUnit.Year => 
                        string.Format(Localization.YearsCountFormat, Internal.DurationLeft),
                    _ => Internal.DurationLeft.ToString(),
                };
            }
        }

        public string EffectsString
        {
            get
            {
                if(Internal?.Effects.Any() == true)
                {
                    return Internal.Effects
                        .Select(EffectsDictionary.GetName)
                        .Aggregate((x, y) => x + ", " + y);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public override string ToString() => Internal?.Name ?? string.Empty;

        public void IncreaseRoundsPassed()
        {
            Internal.DurationPassed++;
            OnPropertyChanged(nameof(RoundsLeft));
        }

        public void DecreaseRoundsPassed()
        {
            Internal.DurationPassed--;
            OnPropertyChanged(nameof(RoundsLeft));
        }

        public void AddEffect(EffectModelBase effect)
        {
            if(effect.InternalModel == null)
            {
                throw new Exception("Не инициализирована модель" + nameof(effect.InternalModel));
            }

            Internal.AddEffect(effect.InternalModel);
            Effects.Add(effect);
        }

        public void UpdateEffect(EffectModelBase effect)
        {
            if (effect.InternalModel == null)
            {
                throw new Exception("Не инициализирована модель" + nameof(effect.InternalModel));
            }

            Internal.UpdateEffect(effect.InternalModel);
            EffectModelBase effectToRemove = Effects.First(x =>
                x.InternalModel?.Number == effect.InternalModel.Number
                && x.InternalModel.GetType().Equals(effect.InternalModel.GetType())
            );
            Effects.Remove(effectToRemove);
            Effects.Add(effect);
        }

        public void RemoveEffect(EffectModelBase effect)
        {
            Internal.RemoveEffect(effect.InternalModel);
            Effects.Remove(effect);
        }
    }
}
