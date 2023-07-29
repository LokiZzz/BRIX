using BRIX.Library.Characters;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Characters
{
    public partial class CharacterProjectVM : ObservableObject
    {
        public CharacterProjectVM() { }

        public CharacterProjectVM(CharacterProject project) 
        { 
            Name = project.Name;
            Description = project.Description;
            Steps = project.Steps;
            CurrentStep = project.CurrentStep;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _steps;
        public int Steps
        {
            get => _steps;
            set
            {
                SetProperty(ref _steps, value);
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(ProgressText));
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
                OnPropertyChanged(nameof(ProgressText));
            }
        }

        public double Progress => (double)CurrentStep / Steps * 100;

        public string ProgressText => $"{CurrentStep}/{Steps}";

        public CharacterProject ToModel()
        {
            return new CharacterProject
            {
                Name = this.Name,
                Description = this.Description,
                Steps = this.Steps,
                CurrentStep = this.CurrentStep
            };
        }
    }
}
