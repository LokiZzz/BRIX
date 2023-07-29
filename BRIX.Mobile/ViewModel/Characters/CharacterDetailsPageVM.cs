﻿using BRIX.Library.Characters;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Details;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterDetailsPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;

        public CharacterDetailsPageVM(ICharacterService characterService, ILocalizationResourceManager localization)
        {
            WeakReferenceMessenger.Default.Register<CurrentCharacterChanged>(
                this,
                async (r, m) => await Initialize()
            );

            _characterService = characterService;
        }

        [ObservableProperty]
        private CharacterModel _character;

        public bool ShowTags => Character?.Tags.Any() == true;

        [RelayCommand]
        public async Task AddTag()
        {
            EntryPopupResult result = await ShowPopupAsync<EntryPopup, EntryPopupResult, EntryPopupParameters>(
                new EntryPopupParameters
                {
                    Title = Localization.MarkOfFate,
                    Message = Localization.MarkOfFateHint,
                    Placeholder = Localization.MarkOfFate,
                    ButtonText = Localization.Add,
                }
            );

            if (result != null && !Character.Tags.Any(x => x.Text == result.Text))
            {
                Character.AddTag(new CharacterTagVM { Text = result.Text });
            }

            await _characterService.UpdateAsync(Character.InternalModel);
            OnPropertyChanged(nameof(ShowTags));
        }

        [RelayCommand]
        public async Task RemoveTag(CharacterTagVM tag)
        {
            Character.RemoveTag(tag);
            await _characterService.UpdateAsync(Character.InternalModel);
            OnPropertyChanged(nameof(ShowTags));
        }

        [RelayCommand]
        public async Task AddProject()
        {
            await Navigation.NavigateAsync<AddOrEditProjectPage>(
                (NavigationParameters.EditMode, EEditingMode.Add)
            );
        }

        [RelayCommand]
        public async Task EditProject(CharacterProjectVM project)
        {
            await Navigation.NavigateAsync<AddOrEditProjectPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.Project, project)
            );
        }

        [RelayCommand]
        public async Task RemoveProject(CharacterProjectVM project)
        {
            AlertPopupResult result = await Ask(string.Format(Localization.AskDeleteProject, project.Name));

            if(result.Answer == EAlertPopupResult.No)
            {
                return;
            }

            Character.RemoveProject(project);
            await _characterService.UpdateAsync(Character.InternalModel);
        }

        public override async Task OnNavigatedAsync()
        {
            await Initialize();
            OnPropertyChanged(nameof(ShowTags));
        }

        private async Task Initialize() 
        {
            Character = new(await _characterService.GetCurrentCharacter());
        }
    }
}
