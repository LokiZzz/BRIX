using BRIX.Library.Characters;
using BRIX.Mobile.Models.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
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


        /// <summary>
        /// Только для удобства тестирования.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task KillThemAll()
        {
            await _characterService.RemoveAllAsync();
            await OnNavigatedAsync();
        }

        [RelayCommand]
        public async Task EditHealth()
        {
            NumericEditorResult result = await ShowPopupAsync<NumericEditorPopup, NumericEditorResult>();

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
        public async Task EditExperience()
        {
            NumericEditorResult result = await ShowPopupAsync<NumericEditorPopup, NumericEditorResult>();

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
            List<Character> characters = await _characterService.GetAllAsync();
            PlayerHaveCharacter = characters.Any();

            if (PlayerHaveCharacter)
            {
                Character = new CharacterModel(characters.FirstOrDefault());
                UpdateExpCards();
            }
            else
            {
                Character = null;
            }
        }

        /// <summary>
        /// В данном случае коллекция ExpCards — это модель представления для двух карточек, отображающих разные 
        /// метрики. Первая показывает опыт до следующего уровня, а вторая непотраченный опыт. К сожалению CarouselView
        /// не умеет вмещать в себя элементы без ItemSource, поэтому применено такое решеиние.
        /// </summary>
        private void UpdateExpCards()
        {
            if (ExpCards == null || !ExpCards.Any())
            {
                ExpCards = new ObservableCollection<ExperienceInfoVM>
                {
                    new ExperienceInfoVM
                    {
                        //Title = _localization[Resources.Localizations.LocalizationKeys.Experience] as string,
                        Title = "Exp to level up",
                        Icon = Awesome.Calculator,
                        IconFont = "Awesome",
                        DoCardActionCommand = new RelayCommand(async () => await EditExperience())
                    },
                    new ExperienceInfoVM
                    {
                        //Title = _localization[Resources.Localizations.LocalizationKeys.Experience] as string,
                        Title = "Free exp",
                        Icon = AwesomeRPG.BurningEmbers,
                        IconFont = "AwesomeRPG",
                        DoCardActionCommand = new RelayCommand(async () => await GoToAbilities())
                    },
                };
            }

            ExpCards.First().Current = Character.Experience;
            ExpCards.First().Target = Character.ExperienceForNextLevel;

            ExpCards.Last().Current = Character.FreeExperience;
            ExpCards.Last().Target = Character.Experience;
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
