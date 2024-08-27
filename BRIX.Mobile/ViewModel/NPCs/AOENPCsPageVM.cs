using BRIX.Lexica;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.NPCs;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.NPCs
{
    public partial class AOENPCsPageVM : ViewModelBase, IQueryAttributable
    {
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
            if (FastNPC.PotentialNPC != null)
            {
                FastNPC.PotentialNPC.Internal.Id = NPC.Internal.Id;
                FastNPC.PotentialNPC.Name = NPC.Name;
                FastNPC.PotentialNPC.Description = NPC.Description;

                NPC = FastNPC.PotentialNPC.Copy() ?? throw new Exception("Ошибка копирования NPC.");
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
                NPC.RemoveAbility(ability);
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

        [RelayCommand]
        private static async Task ShowDescription(CharacterAbilityModel abilityModel)
        {
            await Alert(
                new AlertPopupParameters
                {
                    Mode = EAlertMode.ShowMessage,
                    Title = abilityModel.Name,
                    Message = await abilityModel.Internal.ToFullShortLexis()
                }
            );
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ApplyMode(query);

            HandleBackFormEditing(query);
            query.Clear();
        }

        private void HandleBackFormEditing(IDictionary<string, object> query)
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
            }
        }

        private void ApplyMode(IDictionary<string, object> query)
        {
            if (_mode == EEditingMode.None)
            {
                _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
                NPC = query.GetParameterOrDefault<NPCModel>(NavigationParameters.NPC) ?? new();
                NPC = new NPCModel(NPC.Internal);
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
