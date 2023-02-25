using BRIX.Library.Characters;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Mobile.Resources.Localizations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;

namespace BRIX.Mobile.ViewModel.Characters
{
    public partial class CurrentCharacterPageVM : ViewModelBase
    {
        private readonly ICharacterService _characterService;
        private readonly ILocalizationResourceManager _localization;

        public CurrentCharacterPageVM(ICharacterService characterService, ILocalizationResourceManager localization)
        {
            _characterService = characterService;
            _localization = localization;
        }

        [ObservableProperty]
        private CharacterModel _character;

        [ObservableProperty]
        private bool _playerHaveCharacter;

        [ObservableProperty]
        private bool _showNoCharacterText;

        [ObservableProperty]
        private ObservableCollection<ExperienceInfoVM> _expCards;

        [RelayCommand]
        public async Task Create()
        {
            await Navigation.NavigateAsync<AddOrEditCharacterPage>();
        }

        [RelayCommand]
        public async Task Edit()
        {
            await Navigation.NavigateAsync<AddOrEditCharacterPage>((NavigationParameters.Character, Character));
        }

        [RelayCommand]
        public async Task Switch()
        {
            await Navigation.NavigateAsync(nameof(CharacterListPage));
        }

        [RelayCommand]
        public async Task EditHealth()
        {
            NumericEditorResult result = await ShowPopupAsync<NumericEditorPopup, NumericEditorResult, NumericEditorParameters>(
                new NumericEditorParameters { Title = _localization[LocalizationKeys.Health] as string }
            );

            if (result != null)
            {
                int newHealthValue = result.ToValue(Character.CurrentHealth);

                if (newHealthValue > Character.MaxHealth)
                {
                    Character.CurrentHealth = Character.MaxHealth;
                }
                else if (newHealthValue < 0)
                {
                    Character.CurrentHealth = 0;
                }
                else
                {
                    Character.CurrentHealth = newHealthValue;
                }

                await SaveChanges();
            }
        }

        [RelayCommand]
        public async Task RestoreHealth()
        {
            Character.CurrentHealth = Character.MaxHealth;
            await SaveChanges();
        }

        [RelayCommand]
        public async Task EditExperience()
        {
            NumericEditorResult result = await ShowPopupAsync<NumericEditorPopup, NumericEditorResult, NumericEditorParameters>(
                new NumericEditorParameters { Title = _localization[LocalizationKeys.Experience] as string }
            );

            if (result != null)
            {
                int newEXPValue = result.ToValue(Character.Experience);

                if (newEXPValue >= 0)
                {
                    Character.Experience = newEXPValue;
                    UpdateExpCards();
                    await SaveChanges();
                }
            }
        }

        [RelayCommand]
        private async Task GoToAbilities()
        {
            await Navigation.NavigateAsync<CharacterAbilitiesPage>(mode: ENavigationMode.Absolute);
        }

        public override async Task OnNavigatedAsync()
        {
            IsBusy = true;

            Character character = await _characterService.GetCurrentCharacter();

            if(character != null)
            {
                Character = new CharacterModel(character);
            }
            else
            {
                List<Character> characters = await _characterService.GetAllAsync();
                Character = characters.Any() ? new CharacterModel(characters.First()) : null;
            }

            PlayerHaveCharacter = Character != null;
            ShowNoCharacterText = !PlayerHaveCharacter;

            if (PlayerHaveCharacter)
            {
                UpdateExpCards();
                await _characterService.SelectCurrentCharacter(Character.InternalModel);
            }

            // Возможно такие вызовы уползут в CharacterService, но пока что достаточно этого.
            WeakReferenceMessenger.Default.Send(new ShowCharacterTabsChanged(PlayerHaveCharacter));

            IsBusy = false;
        }

        /// <summary>
        /// В данном случае коллекция ExpCards — это модель представления для двух карточек, отображающих разные 
        /// метрики. Первая показывает опыт до следующего уровня, а вторая не потраченный опыт. К сожалению 
        /// CarouselView не умеет вмещать в себя элементы без ItemSource, поэтому применено такое решение.
        /// </summary>
        private void UpdateExpCards()
        {
            if (ExpCards == null || !ExpCards.Any())
            {
                ExpCards = new ObservableCollection<ExperienceInfoVM>
                {
                    new ExperienceInfoVM
                    {
                        Icon = Awesome.Calculator,
                        IconFont = "Awesome",
                        DoCardActionCommand = new RelayCommand(async () => await EditExperience())
                    },
                    new ExperienceInfoVM
                    {
                        Title = _localization[LocalizationKeys.FreeExperience] as string,
                        Icon = AwesomeRPG.BurningEmbers,
                        IconFont = "AwesomeRPG",
                        DoCardActionCommand = new RelayCommand(async () => await GoToAbilities())
                    },
                };
            }

            ExpCards.First().Current = Character.Experience;
            ExpCards.First().Target = Character.ExperienceForNextLevel;
            ExpCards.First().Title = _localization[LocalizationKeys.ExperienceToLevelup] as string;

            ExpCards.Last().Current = Character.FreeExperience;
            ExpCards.Last().Target = Character.Experience;
            ExpCards.Last().Title = _localization[LocalizationKeys.FreeExperience] as string;
        }

        private async Task SaveChanges()
        {
            await _characterService.UpdateAsync(Character.InternalModel);
        }
    }

    public partial class ExperienceInfoVM : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _icon;

        [ObservableProperty]
        private string _iconFont;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Percent))]
        private int _current;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Percent))]
        private int _target;

        public double Percent => Current / (double)Target;

        public RelayCommand DoCardActionCommand { get; set; }
    }
}
