using BRIX.Library.Effects;
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
    public partial class ChooseEffectPageVM : ViewModelBase
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
                (NavigationParameters.EditMode, EEditingMode.Add)
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
                    EditPage = typeof(DamageEffectPage)
                },
                new()
                {
                    Name = _localization[LocalizationKeys.EffectHeal].ToString(),
                    Icon = AwesomeRPG.HealthIncrease,
                    EditPage = typeof(DamageEffectPage) //временно
                },
                new()
                {
                    Name = "Just win",
                    Icon = AwesomeRPG.FireballSword,
                    EditPage = typeof(DamageEffectPage) //временно
                },
                new()
                {
                    Name = "Win another way",
                    Icon = AwesomeRPG.PoisonCloud,
                    EditPage = typeof(DamageEffectPage) //временно
                },
            };

            return Task.CompletedTask;
        }
    }

    public class EffectToChooseVM
    {
        public string Name{ get; set; }
        public string Icon { get; set; }
        public Type EditPage { get; set; }
    }
}
