using BRIX.Library.Characters;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.NPCs;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class NPCsPageVM(ICharacterService characterService) : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService = characterService;

        private ObservableCollection<NPCItemVM> _npcs = [];
        public ObservableCollection<NPCItemVM> NPCs
        {
            get => _npcs;
            set => SetProperty(ref _npcs, value);
        }

        [RelayCommand]
        public async Task AddNPC()
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Add)
            );
        }


        [RelayCommand]
        public async Task EditNPC()
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit)
            );
        }

        [RelayCommand]
        public async Task RemmoveNPC()
        {
            // Remover NPC
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            
        }

        public override async Task OnNavigatedAsync()
        {
            List<NPC> npcs = await _characterService.GetNPCs();
            NPCs = new(npcs.Select(x => new NPCItemVM { Name = x.Name, Power = x.Power }))
            {
                new NPCItemVM { Name = "Мусорный гоблин", Power = 70 },
                new NPCItemVM { Name = "Дьявольская гидра", Power = 66666 },
                new NPCItemVM { Name = "Крыса по имени Генри", Power = 5 },
            };
        }
    }

    public partial class NPCItemVM
    {
        public string Name { get; set; } = string.Empty;
        public int Power { get; set; }
    }
}
