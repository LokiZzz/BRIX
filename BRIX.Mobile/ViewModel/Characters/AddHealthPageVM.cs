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

        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                SetProperty(ref _health, value);
                Exp = ExperienceCalculator.HealthToExp(value);
            }
        }

        private int _exp;
        public int Exp
        {
            get => _exp;
            set
            {
                SetProperty(ref _exp, value);
                Health = ExperienceCalculator.ExpToHealth(value);
            }
        }

        [RelayCommand]
        public async Task Save()
        {
            Character currentCharacter = await CharacterService.GetCurrentCharacter();
            currentCharacter.ExpInHealth = Exp;
            await CharacterService.UpdateAsync(currentCharacter);
            await Navigation.Back();
        }
    }
}
