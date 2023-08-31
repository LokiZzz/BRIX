using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterStatusesPageVM : ViewModelBase
    {
        public CharacterStatusesPageVM(ICharacterService characterService, IAssetsService assetsService)
        {
            CharacterService = characterService;
            AssetsService = assetsService;
        }

        private Character _currentCharacter;

        public ICharacterService CharacterService { get; }
        public IAssetsService AssetsService { get; }

        [ObservableProperty]
        public ObservableCollection<StatusItemVM> _statuses;

        [RelayCommand]
        public async Task AddStatus()
        {
            await Navigation.NavigateAsync<AOEStatusPage>((NavigationParameters.EditMode, EEditingMode.Add));
        }

        [RelayCommand]
        public async Task EditStatus(StatusItemVM status)
        {
            await Navigation.NavigateAsync<AOEStatusPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.Status, status.Copy())
            );
        }

        [RelayCommand]
        public async Task RemoveStatus(StatusItemVM status)
        {
            Popups.AlertPopupResult result = await Ask(string.Format(Localization.AskDeleteStatus, status.Name));

            if(result.Answer == Popups.EAlertPopupResult.Yes)
            {
                Statuses.Remove(status);
                _currentCharacter.Statuses.Remove(status.Internal);
                await CharacterService.UpdateAsync(_currentCharacter);
                await AssetsService.SaveStatuses(Statuses.Select(x => x.Internal).ToList());
            }
        }

        [RelayCommand]
        public async Task ChangeStatusState(StatusItemVM status)
        {
            if(status == null)
            {
                return;
            }

            if(!_currentCharacter.Statuses.Any(x => x.Name == status.Name) && status.IsActive)
            {
                _currentCharacter.Statuses.Add(status.Internal);
                await CharacterService.UpdateAsync(_currentCharacter);
            }

            if (_currentCharacter.Statuses.Any(x => x.Name == status.Name) && !status.IsActive)
            {
                _currentCharacter.Statuses.Remove(
                    _currentCharacter.Statuses.FirstOrDefault(x => x.Equals(status.Internal))
                );

                await CharacterService.UpdateAsync(_currentCharacter);
            }
        }

        public override async Task OnNavigatedAsync()
        {
            _currentCharacter = await CharacterService.GetCurrentCharacter();
            List<Status> statuses = await AssetsService.GetStatuses();

            Statuses = new(statuses.Select(x => new StatusItemVM(x)));

            foreach (StatusItemVM status in Statuses)
            {
                if(_currentCharacter.Statuses.Any(x => x.Name == status.Name))
                {
                    status.IsActive = true;
                }
            }
        }
    }
}
