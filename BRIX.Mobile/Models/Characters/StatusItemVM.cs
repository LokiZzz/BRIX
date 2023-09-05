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

        public string RoundsLeft => string.Format(Localization.RoundsLeft, Internal.RoundsLeft);

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
            Internal.RoundsPassed++;
            OnPropertyChanged(nameof(RoundsLeft));
        }

        public void DecreaseRoundsPassed()
        {
            Internal.RoundsPassed--;
            OnPropertyChanged(nameof(RoundsLeft));
        }

        public void RemoveEffect(EffectModelBase effect)
        {
            Internal.RemoveEffect(effect.InternalModel);
            Effects.Remove(effect);
        }
    }
}
