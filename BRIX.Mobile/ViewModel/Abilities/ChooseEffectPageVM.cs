using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Abilities.Effects;
using BRIX.Mobile.View.IconFonts;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities
{
    public partial class ChooseEffectPageVM : ViewModelBase, IQueryAttributable
    {
        private readonly ILocalizationResourceManager _localization;

        public ChooseEffectPageVM(ILocalizationResourceManager localization)
        {
            _localization = localization;
        }

        [ObservableProperty]
        private ObservableCollection<EffectToChooseVM> _effects;

        [RelayCommand]
        public async Task Choose(EffectToChooseVM effectToChoose)
        {
            await Navigation.NavigateAsync(
                effectToChoose.EditPage.Name,
                ENavigationMode.Push,
                (NavigationParameters.EditMode, EEditingMode.Add), (NavigationParameters.Ability, _ability)
            );
        }

        public override Task OnNavigatedAsync()
        {
            if (Effects != null) return Task.CompletedTask;

            Effects = new()
            {
                new()
                {
                    Name = _localization[LocalizationKeys.EffectDamage].ToString(),
                    Icon = AwesomeRPG.Sword,
                    EditPage = typeof(HealDamageEffectPage)
                },
                new()
                {
                    Name = _localization[LocalizationKeys.EffectHeal].ToString(),
                    Icon = AwesomeRPG.HealthIncrease,
                    EditPage = typeof(HealDamageEffectPage) //временно
                },
                new()
                {
                    Name = "Just win",
                    Icon = AwesomeRPG.FireballSword,
                    EditPage = typeof(HealDamageEffectPage) //временно
                },
                new()
                {
                    Name = "Win another way",
                    Icon = AwesomeRPG.PoisonCloud,
                    EditPage = typeof(HealDamageEffectPage) //временно
                },
            };

            return Task.CompletedTask;
        }

        private AbilityModel _ability;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability);

            query.Clear();
        }
    }

    public class EffectToChooseVM
    {
        public string Name{ get; set; }
        public string Icon { get; set; }
        public Type EditPage { get; set; }
    }
}
