using BRIX.Library.Characters;
using BRIX.Library.Extensions;
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
                UpdateProgress();
            }
        }

        private int _currentStep;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                SetProperty(ref _currentStep, value);
                UpdateProgress();
            }
        }

        private int _progress;
        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        private string _progressText;
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

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

        private void UpdateProgress()
        {
            Progress = ((double)CurrentStep / Steps * 100).Round();
            ProgressText = $"{CurrentStep}/{Steps}";
        }
    }
}
