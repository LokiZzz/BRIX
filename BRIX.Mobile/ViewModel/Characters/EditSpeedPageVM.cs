using BRIX.Library.Characters;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class EditSpeedPageVM(ICharacterService characterService) : ViewModelBase
    {
        public ICharacterService CharacterService { get; } = characterService;


        private int _expSpent;
        public int ExpSpent
        {
            get => _expSpent;
            set
            {
                //if(SetProperty(ref _expSpent, value) && _invokeHPCalc)
                //{
                //    _invokeEXPCalc = false;
                //    AdditionalHealth = CharacterCalculator.ExpToHealth(_expSpent);
                //    _invokeEXPCalc = true;
                //}

                //OnPropertyChanged(nameof(ExperienceLeft));
            }
        }

        private int _experienceOverall;
        public int ExperienceOverall
        {
            get => _experienceOverall;
            set => SetProperty(ref _experienceOverall, value);
        }

        private int _expSpentOnAbilities;
        public int ExperienceLeft => ExperienceOverall - ExpSpent - _expSpentOnAbilities;

        [RelayCommand]
        public async Task Save()
        {
            //if(ExperienceLeft < 0)
            //{
            //    await Alert(Localization.AddHealthNotEnoughExpAlert);

            //    return;
            //}

            //Character currentCharacter = await CharacterService.GetCurrentCharacterGuaranteed();
            //currentCharacter.ExpInHealth = ExpSpent;
            //await CharacterService.UpdateAsync(currentCharacter);
            //await Navigation.Back();
        }

        public override async Task OnNavigatedAsync()
        {
            //Character currentCharacter = await CharacterService.GetCurrentCharacterGuaranteed();
            //ExpSpent = currentCharacter.ExpInHealth;
            //_rawHealth = currentCharacter.RawMaxHealth;
            //_expSpentOnAbilities = currentCharacter.ExpSpentOnAbilities;
            //OnPropertyChanged(nameof(NewHealth));
            //ExperienceOverall = currentCharacter.Experience;
            //OnPropertyChanged(nameof(ExperienceLeft));
        }
    }
}
