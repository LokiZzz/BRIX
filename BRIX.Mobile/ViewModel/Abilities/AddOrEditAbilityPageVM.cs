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
                (NavigationParameters.EditMode, Mode), 
                (NavigationParameters.Ability, Ability)
            );
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability)
                ?? new AbilityModel(new Ability());

            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            query.Clear();
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
