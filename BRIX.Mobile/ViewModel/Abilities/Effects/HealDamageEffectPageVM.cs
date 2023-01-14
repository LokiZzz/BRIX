using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Library.Extensions;
using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
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

        [ObservableProperty]
        private AbilityModel _ability = new();

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
                _dicePoolToReset = null;
            }
        }

        private DicePool _dicePoolToReset = null;
        private bool _doNotAdjustOnce = false;

        private double _adjustment = 0;
        public double Adjustment
        {
            get => _adjustment;
            set
            {
                if(Math.Abs(_adjustment - value) >= 1)
                {
                    SetProperty(ref _adjustment, value);
                    Adjust(value.Round() * 10);
                }
            }
        }

        private void Adjust(int percent)
        {
            if(_doNotAdjustOnce)
            {
                _doNotAdjustOnce = false;

                return;
            }

            _dicePoolToReset = _dicePoolToReset == null ? HealDamage.Impact.Copy() : _dicePoolToReset;
            HealDamage.Impact = DicePool.FromAdjusted(_dicePoolToReset, percent);
        }


        [RelayCommand]
        private void ApplyAdjustment()
        {
            _dicePoolToReset = null;
            _doNotAdjustOnce = true;
            Adjustment = 0;
        }

        [RelayCommand]
        private void ResetAdjustment()
        {
            HealDamage.Impact = _dicePoolToReset;
            _dicePoolToReset = null;
            _doNotAdjustOnce = true;
            Adjustment = 0;
        }

        public override Task OnNavigatedAsync()
        {
            return Task.CompletedTask;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability) ?? new();
            HealDamage = query.GetParameterOrDefault<HealDamageEffectModel>(NavigationParameters.HealDamageEffect) ?? new();

            Ability.InternalModel.AddEffect(HealDamage.InternalModel);

            if (HealDamage.Impact.IsEmpty)
            {
                HealDamage.Impact = new DicePool((1, 4));
            }

            query.Clear();
        }
    }
}
