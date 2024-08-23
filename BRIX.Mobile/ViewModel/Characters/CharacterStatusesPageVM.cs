using BRIX.Library.Abilities;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CharacterStatusesPageVM(ICharacterService characterService, IAssetsService assetsService) 
        : ViewModelBase, IQueryAttributable
    {
        private readonly CharacterModel? _currentCharacter;

        public ICharacterService CharacterService { get; } = characterService;
        public IAssetsService AssetsService { get; } = assetsService;

        [ObservableProperty]
        public ObservableCollection<StatusItemVM> _statuses = [];

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
            Popups.AlertPopupResult? result = await Ask(string.Format(Localization.AskDeleteStatus, status.Name));

            if(result?.Answer == Popups.EAlertPopupResult.Yes)
            {
                Statuses.Remove(status);
                await AssetsService.SaveStatuses(Statuses.Select(x => x.Internal).ToList());
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            StatusItemVM? status = query.GetParameterOrDefault<StatusItemVM>(NavigationParameters.Status);
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
                        Status existingStatus = statuses.Single(x => x.Equals(status.Internal));
                        statuses[statuses.IndexOf(existingStatus)] = status.Internal;

                        if(_currentCharacter == null)
                        {
                            throw new Exception("Текущий персонаж не инициализирован.");
                        }

                        //_currentCharacter.ReplaceStatus(status);
                        await CharacterService.UpdateAsync(_currentCharacter.InternalModel);
                        break;
                }

                await AssetsService.SaveStatuses(statuses);
                Statuses = new(statuses.Select(x => new StatusItemVM(x)));
            }

            query?.Clear();
        }
    }
}
