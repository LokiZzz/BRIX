using BRIX.Library.Characters;
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ApplyMode(query);

            switch(_mode)
            {
                case EEditingMode.Edit:
                    NPC = query.GetParameterOrDefault<NPCModel>(NavigationParameters.NPC) ?? new();
                    break;
            }

            query.Clear();
        }

        private void ApplyMode(IDictionary<string, object> query)
        {
            _mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);

            Title = _mode switch
            {
                EEditingMode.Add => Localization.AddNPC,
                EEditingMode.Edit => Localization.EditNPC,
                _ => string.Empty
            };
        }
    }
}
