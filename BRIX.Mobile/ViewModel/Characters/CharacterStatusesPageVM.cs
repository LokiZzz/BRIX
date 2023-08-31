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
    public partial class CharacterStatusesPageVM : ViewModelBase, IQueryAttributable
    {
        public CharacterStatusesPageVM(ICharacterService characterService, IAssetsService assetsService)
        {
            CharacterService = characterService;
            AssetsService = assetsService;
        }

        private CharacterModel _currentCharacter;

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
                _currentCharacter.AddStatus(status);
                await CharacterService.UpdateAsync(_currentCharacter.InternalModel);
            }

            if (_currentCharacter.Statuses.Any(x => x.Name == status.Name) && !status.IsActive)
            {
                _currentCharacter.InternalModel.Statuses.Remove(
                    _currentCharacter.InternalModel.Statuses.FirstOrDefault(x => x.Equals(status.Internal))
                );

                await CharacterService.UpdateAsync(_currentCharacter.InternalModel);
            }
        }

        public override async Task OnNavigatedAsync()
        {
            _currentCharacter = new(await CharacterService.GetCurrentCharacter());
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

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            StatusItemVM status = query.GetParameterOrDefault<StatusItemVM>(NavigationParameters.Status);
            EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

            if (status != null)
            {
                List<Status> statuses = await AssetsService.GetStatuses();

                switch (mode)
                {
                    case EEditingMode.Add:
                        statuses.Add(status.Internal);
                        break;
                    case EEditingMode.Edit:
                        Status existingStatus = statuses.FirstOrDefault(x => x.Equals(status.Internal));
                        statuses[statuses.IndexOf(existingStatus)] = status.Internal;
                        _currentCharacter.ReplaceStatus(status);
                        break;
                }

                await AssetsService.SaveStatuses(statuses);
                Statuses = new(statuses.Select(x => new StatusItemVM(x)));
            }
        }
    }
}
