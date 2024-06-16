using BRIX.Library.Abilities;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Characters;
using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.NPCs;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class AOENPCsPageVM(ICharacterService characterService) : ViewModelBase, IQueryAttributable
    {
        private readonly ICharacterService _characterService = characterService;

        private EEditingMode _mode;

        private NPCModel _npc = new ();
        public NPCModel NPC
        {
            get => _npc;
            set => SetProperty(ref _npc, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private FastNPCCreationVM _fastNPC = new();
        public FastNPCCreationVM FastNPC
        {
            get => _fastNPC;
            set => SetProperty(ref _fastNPC, value);
        }

        [RelayCommand]
        private void FastCreate()
        {
            NPCModel? potentialNPC = FastNPC.PotentialNPC.Copy();

            if (potentialNPC != null)
            {
                NPC = potentialNPC;
            }
            
        }

        [RelayCommand]
        private async Task AddAbility()
        {
            await Navigation.NavigateAsync<AOEAbilityPage>(
                (NavigationParameters.EditMode, EEditingMode.Add)
            );
        }

        [RelayCommand]
        private async Task EditAbility(CharacterAbilityModel ability)
        {
            await Navigation.NavigateAsync<AOEAbilityPage>(
                (NavigationParameters.Ability, ability.Copy()),
                (NavigationParameters.EditMode, EEditingMode.Edit)
            );
        }

        [RelayCommand]
        private async Task RemoveAbility(CharacterAbilityModel ability)
        {
            AlertPopupResult? result = await Ask(Localization.DeleteAbilityQuestion);

            if (result?.Answer == EAlertPopupResult.Yes)
            {
                NPC.RemoveAbility(ability.InternalModel.Id);
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            await Navigation.Back(stepsBack: 1, 
                (NavigationParameters.NPC, NPC),
                (NavigationParameters.EditMode, _mode)
            );
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ApplyMode(query);

            await HandleBackFormEditing(query);
            query.Clear();
        }

        private async Task HandleBackFormEditing(IDictionary<string, object> query)
        {
            CharacterAbilityModel? editedAbility =
                query.GetParameterOrDefault<CharacterAbilityModel>(NavigationParameters.Ability);

            if (editedAbility != null && NPC != null)
            {
                EEditingMode mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

                switch (mode)
                {
                    case EEditingMode.Add:
                        NPC.AddAbility(editedAbility);
                        break;
                    case EEditingMode.Edit:
                        NPC.UpdateAbility(editedAbility);
                        break;
                }

                await _characterService.UpdateNPC(NPC.Internal);
            }
        }

        private void ApplyMode(IDictionary<string, object> query)
        {
            if (_mode == EEditingMode.None)
            {
                _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
                NPC = query.GetParameterOrDefault<NPCModel>(NavigationParameters.NPC) ?? new();
            }

            Title = _mode switch
            {
                EEditingMode.Add => Localization.AddNPC,
                EEditingMode.Edit => Localization.EditNPC,
                _ => string.Empty
            };
        }
    }
}
