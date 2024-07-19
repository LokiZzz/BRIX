using BRIX.Library.Characters;
using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Library.Extensions;

namespace BRIX.Mobile.Models.Characters
{
    public partial class CharacterSpeedModel : ObservableObject
    {
        public CharacterSpeed InternalModel { get; set; }

        private readonly Action? UpdateCostDelegate;

        public CharacterSpeedModel(CharacterSpeed internalModel, Action? updateCostDelegate = null)
        {
            InternalModel = internalModel;
            UpdateCostDelegate = updateCostDelegate;
            UpdateCost();
        }

        public int SpeedEXPCost => InternalModel.GetExpCost();

        public double Walk
        {
            get => InternalModel.Walk;
            set
            {
                SetProperty(InternalModel.Walk, value, InternalModel, (character, x) => character.Walk = x);
                OnPropertyChanged(nameof(WalkMAP));
                UpdateCost();
            }
        }

        public double WalkMAP => (Walk / 5).Round(2);

        public double Swim
        {
            get => InternalModel.Swim;
            set
            {
                SetProperty(InternalModel.Swim, value, InternalModel, (character, x) => character.Swim = x);
                OnPropertyChanged(nameof(SwimMAP));
                UpdateCost();
            }
        }

        public double SwimMAP => (Swim / 5).Round(2);

        public double Climb
        {
            get => InternalModel.Climb;
            set
            {
                SetProperty(InternalModel.Climb, value, InternalModel, (character, x) => character.Climb = x);
                OnPropertyChanged(nameof(ClimbMAP));
                UpdateCost();
            }
        }

        public double ClimbMAP => (Climb / 5).Round(2);

        public double Fly
        {
            get => InternalModel.Fly;
            set
            {
                SetProperty(InternalModel.Fly, value, InternalModel, (character, x) => character.Fly = x);
                OnPropertyChanged(nameof(FlyMAP));
                UpdateCost();
            }
        }

        public double FlyMAP => (Fly / 5).Round(2);

        public double Burrow
        {
            get => InternalModel.Burrow;
            set
            {
                SetProperty(InternalModel.Burrow, value, InternalModel, (character, x) => character.Burrow = x);
                OnPropertyChanged(nameof(BurrowMAP));
                UpdateCost();
            }
        }

        public double BurrowMAP => (Burrow / 5).Round(2);

        public double Teleportation
        {
            get => InternalModel.Teleportation;
            set
            {
                SetProperty(InternalModel.Teleportation, value, InternalModel, (character, x) => character.Teleportation = x);
                OnPropertyChanged(nameof(TeleportationMAP));
                UpdateCost();
            }
        }

        public double TeleportationMAP => (Teleportation / 5).Round(2);

        private void UpdateCost()
        {
            OnPropertyChanged(nameof(SpeedEXPCost));
            UpdateCostDelegate?.Invoke();
        }
    }
}
