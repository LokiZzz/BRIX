using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Characters
{
    public partial class CharacterProjectVM : ObservableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private int _steps;
        public int Steps
        {
            get => _steps;
            set
            {
                SetProperty(ref _steps, value);
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(Text));
            }
        }

        private int _currentStep;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                SetProperty(ref _currentStep, value);
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(Text));
            }
        }

        public double Progress => (double)CurrentStep / Steps * 100;
        public string Text => $"{CurrentStep}/{Steps}";
    }
}
