using BRIX.Library;
using BRIX.Library.Characters;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.View.Abilities;
using BRIX.Utility.Extensions;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class AddOrEditAbilityPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;

        public AddOrEditAbilityPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

        [ObservableProperty]
        private AbilityModel _ability;

        [ObservableProperty]
        private EEditingMode _mode;

        [ObservableProperty]
        private string _title;

        [RelayCommand]
        public async Task Save()
        {
            await Navigation.Back(
                stepsBack: 1,
                (NavigationParameters.EditMode, Mode), 
                (NavigationParameters.Ability, Ability)
            );
        }

        [RelayCommand]
        public async Task AddEffect()
        {
            await Navigation.NavigateAsync<ChooseEffectPage>((NavigationParameters.Ability, Ability.Copy()));
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability)
                ?? new AbilityModel(new Ability());
            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            await HandleBackFromEditing(query);
            query.Clear();
        }

        private async Task HandleBackFromEditing(IDictionary<string, object> query)
        {
            EffectBase editedEffect = query.GetParameterOrDefault<EffectBase>(NavigationParameters.Effect);

            if(editedEffect != null)
            {
                Ability.InternalModel.AddEffect(editedEffect);
            }
        }

        public override Task OnNavigatedAsync()
        {
            switch(Mode)
            {
                case EEditingMode.Add:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Add].ToString();
                    break;
                case EEditingMode.Edit:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Edit].ToString();
                    break;
                case EEditingMode.Upgrade:
                    Title = _localization[LocalizationKeys.AddOrEditAbilityPageTitle_Upgrade].ToString();
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
