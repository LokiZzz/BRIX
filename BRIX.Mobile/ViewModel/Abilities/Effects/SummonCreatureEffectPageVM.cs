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
        public async Task AddCreature()
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Add)
            );
        }


        [RelayCommand]
        public async Task EditCreature(SummoningCreaturesVM item)
        {
            await Navigation.NavigateAsync<AOENPCsPage>(
                (NavigationParameters.EditMode, EEditingMode.Edit),
                (NavigationParameters.NPC, item.Creature.Copy())
            );
        }

        [RelayCommand]
        public async Task RemoveCreature(SummoningCreaturesVM item)
        {
            AlertPopupResult? result = await Ask(Localization.RemoveNPCQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                if (Effect != null)
                {
                    Effect.RemoveCreature(item);
                    CostMonitor?.UpdateCost();
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
                        Effect?.AddCreature(npc);
                        break;
                    case EEditingMode.Edit:
                        Effect?.UpdateCreature(npc);
                        break;
                }

                CostMonitor?.UpdateCost();
            }
        }
    }
}
