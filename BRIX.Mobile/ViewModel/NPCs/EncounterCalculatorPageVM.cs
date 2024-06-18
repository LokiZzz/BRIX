using BRIX.Library.Enums;
using BRIX.Library.Extensions;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Settings;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class EncounterCalculatorPageVM : ViewModelBase
    {
        private ObservableCollection<CalculatorItemVM> _party = [];
        public ObservableCollection<CalculatorItemVM> Party
        {
            get => _party;
            set => SetProperty(ref _party, value);
        }

        public bool ShowCharacters => Party.Count > 0;

        private ObservableCollection<CalculatorItemVM> _npcs = [];
        public ObservableCollection<CalculatorItemVM> NPCs
        {
            get => _npcs;
            set => SetProperty(ref _npcs, value);
        }

        public bool ShowNPC => NPCs.Count > 0;

        private int _characterCount = 1;
        public int CharacterCount
        {
            get => _characterCount;
            set => SetProperty(ref _characterCount, value);
        }

        private int _characterExp;
        public int CharacterExp
        {
            get => _characterExp;
            set => SetProperty(ref _characterExp, value);
        }

        private int _npcCount = 1;
        public int NPCCount
        {
            get => _npcCount;
            set => SetProperty(ref _npcCount, value);
        }

        private int _npcExp;
        public int NPCExp
        {
            get => _npcExp;
            set => SetProperty(ref _npcExp, value);
        }

        private string _difficultyText = string.Empty;
        public string DifficultyText
        {
            get => _difficultyText;
            set => SetProperty(ref _difficultyText, value);
        }

        private string _versusText = string.Empty;
        public string VersusText
        {
            get => _versusText;
            set => SetProperty(ref _versusText, value);
        }

        private EEncounterDifficulty _difficulty;
        public EEncounterDifficulty Difficulty
        {
            get => _difficulty;
            set => SetProperty(ref _difficulty, value);
        }

        [RelayCommand]
        public void AddCharacter()
        {
            Party.Add(new CalculatorItemVM { Count = CharacterCount, Power = CharacterExp });
            Preferences.Set(Other.Party, JsonConvert.SerializeObject(Party));
            OnPropertyChanged(nameof(ShowCharacters));
            UpdateStatistics();
        }

        [RelayCommand]
        public void RemoveCharacter(CalculatorItemVM item)
        {
            Party.Remove(item);
            Preferences.Set(Other.Party, JsonConvert.SerializeObject(Party));
            OnPropertyChanged(nameof(ShowCharacters));
            UpdateStatistics();
        }

        [RelayCommand]
        public void AddNPC()
        {
            NPCs.Add(new CalculatorItemVM { Count = NPCCount, Power = NPCExp });
            OnPropertyChanged(nameof(ShowNPC));
            UpdateStatistics();
        }

        [RelayCommand]
        public void RemoveNPC(CalculatorItemVM item)
        {
            NPCs.Remove(item);
            OnPropertyChanged(nameof(ShowNPC));
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            int charactersPower = Party.Sum(x => x.Count * x.Power);
            int npcsPower = NPCs.Sum(x => x.Count * x.Power);

            if(charactersPower == 0 || npcsPower == 0)
            {
                Difficulty = EEncounterDifficulty.Normal;
                DifficultyText = Localization.NormalDifficulty;
                VersusText = string.Empty;

                return;
            }

            int percent = ((double)(npcsPower - charactersPower) / charactersPower * 100).Round();
            string percentString = percent > 0 ? $"+{percent}" : $"{percent}";
            VersusText = $"{charactersPower} vs {npcsPower} ({percentString}%)";

            double coef = npcsPower / charactersPower;
            Difficulty = coef switch
            {
                <= 0.5 => EEncounterDifficulty.Easy,
                <= 1 => EEncounterDifficulty.Normal,
                <= 2 => EEncounterDifficulty.Hard,
                > 2 => EEncounterDifficulty.Nightmare,
                _ => throw new Exception("Ошибка расчёта сложности столкновения.")
            };

            DifficultyText = Difficulty switch
            {
                EEncounterDifficulty.Easy => Localization.EasyDifficulty,
                EEncounterDifficulty.Normal => Localization.NormalDifficulty,
                EEncounterDifficulty.Hard => Localization.HardDifficulty,
                EEncounterDifficulty.Nightmare => Localization.NightmareDifficulty,
                _ => throw new Exception("Не найден текст уровня сложности столкновения.")
            };
        }

        public override Task OnNavigatedAsync()
        {
            string savedCharacters = Preferences.Get(Other.Party, string.Empty);

            if(!string.IsNullOrEmpty(savedCharacters))
            {
                Party = JsonConvert.DeserializeObject<ObservableCollection<CalculatorItemVM>>(savedCharacters)
                    ?? throw new Exception("Ошибка десериализации сохранённой партии персонажей.");
            }

            OnPropertyChanged(nameof(ShowCharacters));
            OnPropertyChanged(nameof(ShowNPC));

            UpdateStatistics();

            return Task.CompletedTask;
        }
    }

    public class CalculatorItemVM
    {
        public int Count { get; set; }
        public int Power { get; set; }
    }
}
