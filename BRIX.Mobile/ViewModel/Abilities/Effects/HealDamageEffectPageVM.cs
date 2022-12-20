using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class HealDamageEffectPageVM : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private EEditingMode _mode;

        [ObservableProperty]
        private HealDamageEffectModel _healDamage = new();

        [RelayCommand]
        private async Task EditFormula()
        {
            string formula = HealDamage?.InternalModel?.Impact?.ToString() ?? string.Empty;
            DiceValuePopupResult result = await ShowPopupAsync<DiceValuePopup, DiceValuePopupResult, DiceValuePopupParameters>(
                new DiceValuePopupParameters { Formula = formula }
            );

            if (result != null)
            {
                HealDamage.Impact = result.DicePool;
            }
        }

        public override Task OnNavigatedAsync()
        {
            return Task.CompletedTask;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            HealDamage = query.GetParameterOrDefault<HealDamageEffectModel>(NavigationParameters.HealDamageEffect) ?? new();

            query.Clear();
        }
    }
}
