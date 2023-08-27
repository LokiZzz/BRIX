using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Library.Ability;
using BRIX.Mobile.Resources.Localizations;

namespace BRIX.Mobile.Models.Characters
{
    public partial class StatusItemVM : ObservableObject
    {
        public StatusItemVM(Status status)
        {
            Internal = status;
        }

        public Status Internal { get; set; }

        public string Name
        {
            get => Internal.Name;
            set => SetProperty(Internal.Name, value, Internal, (status, name) => status.Name = name);
        }

        public string RoundsLeft => string.Format(Localization.RoundsLeft, Internal.RoundsLeft);

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
    }
}
