using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AddHealthPageVM : ViewModelBase
    {
        public ICharacterService CharacterService { get; }

        public AddHealthPageVM(ICharacterService characterService)
        {
            CharacterService = characterService;
        }

        private bool _invokeHPCalc = true;
        private bool _invokeEXPCalc = true;

        private int _additionalHealth;
        public int AdditionalHealth
        {
            get => _additionalHealth;
            set
            {
                if (SetProperty(ref _additionalHealth, value) && _invokeEXPCalc)
                {
                    _invokeHPCalc = false;
                    ExpSpent = CharacterCalculator.HealthToExp(_additionalHealth);
                    _invokeHPCalc = true;
                }

                OnPropertyChanged(nameof(NewHealth));
            }
        }

        private int _expSpent;
        public int ExpSpent
        {
            get => _expSpent;
            set
            {
                if(SetProperty(ref _expSpent, value) && _invokeHPCalc)
                {
                    _invokeEXPCalc = false;
                    AdditionalHealth = CharacterCalculator.ExpToHealth(_expSpent);
                    _invokeEXPCalc = true;
                }

                OnPropertyChanged(nameof(ExperienceLeft));
            }
        }

        private int _rawHealth;
        public int NewHealth => _rawHealth + AdditionalHealth;

        public int ExperienceOverall { get; set; }

        public int ExperienceLeft => ExperienceOverall - ExpSpent;

        [RelayCommand]
        public async Task Save()
        {
            Character currentCharacter = await CharacterService.GetCurrentCharacter();
            currentCharacter.ExpInHealth = ExpSpent;
            await CharacterService.UpdateAsync(currentCharacter);
            await Navigation.Back();
        }

        [RelayCommand]
        public void AddHealth(string add)
        {
            AdditionalHealth = _additionalHealth + int.Parse(add);
        }

        [RelayCommand]
        public async Task SpendAllExp()
        {
            Character currentCharacter = await CharacterService.GetCurrentCharacter();
            ExpSpent = currentCharacter.ExpInHealth + currentCharacter.AvailableExp;
        }

        [RelayCommand]
        public void Reset()
        {
            ExpSpent = 0;
        }

        public override async Task OnNavigatedAsync()
        {
            Character currentCharacter = await CharacterService.GetCurrentCharacter();
            ExpSpent = currentCharacter.ExpInHealth;
            _rawHealth = currentCharacter.RawHealth;
            OnPropertyChanged(nameof(NewHealth));
            ExperienceOverall = currentCharacter.Experience;
            OnPropertyChanged(nameof(ExperienceOverall));
            OnPropertyChanged(nameof(ExperienceLeft));
        }
    }
}
