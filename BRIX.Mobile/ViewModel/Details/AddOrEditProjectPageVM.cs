using BRIX.Library.Characters;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Details
{
    public partial class AddOrEditProjectPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService;

        public AddOrEditProjectPageVM(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        EEditingMode _mode;

        private string _title = string.Empty;
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

        private CharacterProjectVM _project = new();
        public CharacterProjectVM Project
        {
            get => _project;
            set => SetProperty(ref _project, value);
        }

        [RelayCommand]
        public async Task Save()
        {
            Character character = await _characterService.GetCurrentCharacterGuaranteed();

            switch (_mode)
            {
                case EEditingMode.Add:
                    if(character.Projects.Any(x => x.Name == Project.Name))
                    {
                        await Alert(Localization.SameProjectExistsWarning);

                        return;
                    }
                    character.Projects.Add(Project.ToModel());
                    break;
                case EEditingMode.Edit:
                    character.Projects.Remove(character.Projects.Single(x => x.Name == Project.Name));
                    character.Projects.Add(Project.ToModel());
                    break;
            }

            await _characterService.UpdateAsync(character);
            await Navigation.Back();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            Project = query.GetParameterOrDefault<CharacterProjectVM>(NavigationParameters.Project)
                ?? new CharacterProjectVM();

            switch (_mode)
            {
                case EEditingMode.Add:
                    Title = Localization.AddProject;
                    break;
                case EEditingMode.Edit:
                    Title = Localization.EditProject;
                    break;
            }
        }
    }
}
