using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Models.NPCs;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.NPCs;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class SummonCreatureEffectPageVM : EffectPageVMBase<SummonCreatureEffectModel>
    {
        [RelayCommand]
        public async Task AddNPC()
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Add)
            );
        }


        [RelayCommand]
        public async Task EditNPC(SummoningCreaturesVM item)
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.NPC, item.Creature.Copy())
            );
        }

        [RelayCommand]
        public async Task RemoveNPC(SummoningCreaturesVM item)
        {
            AlertPopupResult? result = await Ask(Localization.RemoveNPCQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                if (Effect != null)
                {
                    
                }
            }
        }

        protected override void HandleBack(IDictionary<string, object> query)
        {
            HandleBackFormEditing(query);
            query.Clear();
        }

        private void HandleBackFormEditing(IDictionary<string, object> query)
        {
            NPCModel? npc = query.GetParameterOrDefault<NPCModel>(NavigationParameters.NPC);

            if (npc != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch (mode)
                {
                    case EEditingMode.Add:
                        Effect?.Creatures.Add(new SummoningCreaturesVM { Creature = npc });
                        Effect.Inter
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
