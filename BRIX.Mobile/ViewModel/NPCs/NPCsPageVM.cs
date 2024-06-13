using BRIX.Library.Characters;
using BRIX.Mobile.Models.NPCs;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.NPCs;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class NPCsPageVM(ICharacterService characterService) : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService = characterService;

        private ObservableCollection<NPCModel> _npcs = [];
        public ObservableCollection<NPCModel> NPCs
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
        public async Task EditNPC(NPCModel item)
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.NPC, item.Copy())
            );
        }

        [RelayCommand]
        public async Task RemoveNPC(NPCModel item)
        {
            AlertPopupResult? result = await Ask(Localization.RemoveNPCQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                await _characterService.RemoveNPC(item.Internal);
                NPCs.Remove(item);
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await HandleBackFormEditing(query);
            query.Clear();
        }

        private async Task HandleBackFormEditing(IDictionary<string, object> query)
        {
            NPCModel? npc = query.GetParameterOrDefault<NPCModel>(NavigationParameters.NPC);

            if (npc != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch (mode)
                {
                    case EEditingMode.Add:
                        await _characterService.AddNPC(npc.Internal);
                        NPCs.Add(npc);
                        break;
                    case EEditingMode.Edit:
                        await _characterService.UpdateNPC(npc.Internal);
                        int index = NPCs.IndexOf(NPCs.First(x => x.Internal.Id == npc.Internal.Id));
                        NPCs[index] = npc;
                        break;
                }
            }
        }

        public override async Task OnNavigatedAsync()
        {
            List<NPC> npcs = await _characterService.GetNPCs();
            NPCs = new(npcs.Select(x => new NPCModel(x)));
        }
    }
}
