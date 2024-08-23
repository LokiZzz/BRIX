using BRIX.Lexica.Templates.en_US;
using BRIX.Library.Abilities;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class AddHealthPageVM(ICharacterService characterService) : ViewModelBase
    {
        public ICharacterService CharacterService { get; } = characterService;

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
        public int NewHealth => _rawHealth + AdditionalHealth + Modificator;

        private int _experienceOverall;
        public int ExperienceOverall
        {
            get => _experienceOverall;
            set => SetProperty(ref _experienceOverall, value);
        }

        private int _expSpentOnAbilities;
        public int ExperienceLeft => ExperienceOverall - ExpSpent - _expSpentOnAbilities;

        private int _modificator;
        public int Modificator
        {
            get => _modificator;
            set
            {
                SetProperty(ref _modificator, value);
                OnPropertyChanged(nameof(NewHealth));
            }
        }

        [RelayCommand]
        public async Task Save()
        {
            if(ExperienceLeft < 0)
            {
                await Alert(Localization.AddHealthNotEnoughExpAlert);

                return;
            }

            Character currentCharacter = await CharacterService.GetCurrentCharacterGuaranteed();
            currentCharacter.ExpInHealth = ExpSpent;

            HandleModificator(currentCharacter);

            await CharacterService.UpdateAsync(currentCharacter);
            await Navigation.Back();
        }

        private void HandleModificator(Character currentCharacter)
        {
            if(Modificator == _oldModificator || NewHealth < 0)
            {
                return;
            }

            currentCharacter.Statuses.Clear();
            Status status = new();

            if(Modificator > 0)
            {
                status.AddEffect(new FortifyEffect { Impact = new DicePool(Modificator) });
                currentCharacter.CurrentHealth += Modificator - _oldModificator;
            }
            else if(Modificator < 0)
            {
                status.AddEffect(new ExhaustionEffect { Impact = new DicePool(-Modificator) });

                if(currentCharacter.CurrentHealth > currentCharacter.MaxHealth)
                {
                    currentCharacter.CurrentHealth = currentCharacter.MaxHealth;
                }
            }

            if (status.Effects.Count > 0)
            {
                currentCharacter.Statuses.Enqueue(status);
            }
        }

        [RelayCommand]
        public void AddHealth(string add)
        {
            AdditionalHealth = _additionalHealth + int.Parse(add);
        }

        [RelayCommand]
        public async Task SpendAllExp()
        {
            Character currentCharacter = await CharacterService.GetCurrentCharacterGuaranteed();
            ExpSpent = currentCharacter.ExpInHealth + currentCharacter.AvailableExp;
        }

        [RelayCommand]
        public void Reset()
        {
            ExpSpent = 0;
        }

        [RelayCommand]
        public void ResetTemp()
        {
            Modificator = 0;
        }

        private int _oldModificator = 0;

        public override async Task OnNavigatedAsync()
        {
            Character currentCharacter = await CharacterService.GetCurrentCharacterGuaranteed();
            ExpSpent = currentCharacter.ExpInHealth;
            _rawHealth = currentCharacter.RawMaxHealth;
            _expSpentOnAbilities = currentCharacter.ExpSpentOnAbilities;
            OnPropertyChanged(nameof(NewHealth));
            ExperienceOverall = currentCharacter.Experience;
            OnPropertyChanged(nameof(ExperienceLeft));

            Status? fortifyStatus = currentCharacter.Statuses.FirstOrDefault(x => x.Effects.Any(y => y is FortifyEffect));
            FortifyEffect? fortifyEffect = fortifyStatus?.Effects.FirstOrDefault(x => x is FortifyEffect) as FortifyEffect;

            if(fortifyEffect != null)
            {
                Modificator = fortifyEffect.Impact.Average();
            }

            Status? exhStatus = currentCharacter.Statuses.FirstOrDefault(x => x.Effects.Any(y => y is ExhaustionEffect));
            ExhaustionEffect? exhEffect = exhStatus?.Effects.FirstOrDefault(x => x is ExhaustionEffect) as ExhaustionEffect;

            if (exhEffect != null)
            {
                Modificator = -exhEffect.Impact.Average();
            }

            _oldModificator = Modificator;
        }
    }
}
