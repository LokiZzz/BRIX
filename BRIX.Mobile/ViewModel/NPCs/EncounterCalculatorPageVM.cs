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

        [RelayCommand]
        public void AddCharacter()
        {
            Party.Add(new CalculatorItemVM { Count = $"{CharacterCount}x", Power = $"{CharacterExp} exp" });
            Preferences.Set(Other.Party, JsonConvert.SerializeObject(Party));
            OnPropertyChanged(nameof(ShowCharacters));
        }

        [RelayCommand]
        public void RemoveCharacter(CalculatorItemVM item)
        {
            Party.Remove(item);
            Preferences.Set(Other.Party, JsonConvert.SerializeObject(Party));
            OnPropertyChanged(nameof(ShowCharacters));
        }

        [RelayCommand]
        public void AddNPC()
        {
            NPCs.Add(new CalculatorItemVM { Count = $"{NPCCount}x", Power = $"{NPCExp} exp" });
            OnPropertyChanged(nameof(ShowNPC));
        }

        [RelayCommand]
        public void RemoveNPC(CalculatorItemVM item)
        {
            NPCs.Remove(item);
            OnPropertyChanged(nameof(ShowNPC));
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

            return Task.CompletedTask;
        }
    }

    public class CalculatorItemVM
    {
        public string Count { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
    }
}
