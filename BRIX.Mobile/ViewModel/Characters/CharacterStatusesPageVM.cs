using BRIX.Library.Ability;
using BRIX.Library.Aspects;
using BRIX.Library.Characters;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
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

        private Character _currentCharacter;

        public ICharacterService CharacterService { get; }
        public IAssetsService AssetsService { get; }

        [ObservableProperty]
        public ObservableCollection<StatusItemVM> _statuses;

        [RelayCommand]
        public async Task AddStatus()
        {
            //Navigate to add status
        }

        [RelayCommand]
        public async Task EditStatus(StatusItemVM status)
        {
            //Navigate to add status
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            
        }

        public override async Task OnNavigatedAsync()
        {
            _currentCharacter = await CharacterService.GetCurrentCharacter();
            List<Status> statuses = await AssetsService.GetStatuses();

            //УБРАТЬ
            if (!statuses.Any())
            {
                Status dragonFortitude = new Status() { Name = "Драконья крепкость" };
                FortifyEffect fortify = new FortifyEffect() { Impact = new(5) };
                fortify.GetAspect<RoundDurationAspect>().Rounds = 4;
                FortifyEffect fortify2 = new FortifyEffect() { Impact = new(10) };
                fortify.GetAspect<RoundDurationAspect>().Rounds = 8;
                dragonFortitude.AddEffect(fortify);
                dragonFortitude.AddEffect(fortify2);
                dragonFortitude.RoundsPassed = 1;
                statuses.Add(dragonFortitude);
                Status ill = new Status() { Name = "Ветрянка" };
                ExhaustionEffect exhaustion = new ExhaustionEffect() { Impact = new(3) };
                exhaustion.GetAspect<RoundDurationAspect>().Rounds = 4;
                ill.AddEffect(exhaustion);
                statuses.Add(ill);
                await AssetsService.SaveStatuses(statuses);
            }

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
