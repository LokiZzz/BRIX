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
    public partial class StatusItemVM : ObservableObject
    {
        public StatusItemVM(Status status)
        {
            Internal = status;
            Effects = new ObservableCollection<EffectModelBase>(
                status.Effects.Select(EffectModelFactory.GetModel)
            );
        }

        public Status Internal { get; set; }

        public ObservableCollection<EffectModelBase> Effects { get; set; } = new ObservableCollection<EffectModelBase>();

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
                switch(Internal.GetHighestTimeUnit())
                {
                    case Library.Enums.ETimeUnit.Round:
                        return string.Format(Localization.RoundsCountFormat, Internal.DurationLeft);
                    case Library.Enums.ETimeUnit.Minute:
                        return string.Format(Localization.MinutesCountFormat, Internal.DurationLeft);
                    case Library.Enums.ETimeUnit.Hour:
                        return string.Format(Localization.HoursCountFormat, Internal.DurationLeft);
                    case Library.Enums.ETimeUnit.Day:
                        return string.Format(Localization.DaysCountFormat, Internal.DurationLeft);
                    case Library.Enums.ETimeUnit.Year:
                        return string.Format(Localization.YearsCountFormat, Internal.DurationLeft);
                    default:
                        return Internal.DurationLeft.ToString();
                }
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

        public override string ToString() => Internal?.Name;

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
            Internal.AddEffect(effect.InternalModel);
            Effects.Add(effect);
        }

        public void UpdateEffect(EffectModelBase effect)
        {
            Internal.UpdateEffect(effect.InternalModel);
            EffectModelBase effectToRemove = Effects.First(x =>
                x.InternalModel.Number == effect.InternalModel.Number
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
