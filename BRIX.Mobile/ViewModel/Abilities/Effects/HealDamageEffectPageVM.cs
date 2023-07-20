using BRIX.Library.DiceValue;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class DamageEffectPageVM : EffectPageVMBase<DamageEffectModel>
    {
        [RelayCommand]
        private async Task EditFormula()
        {
            string formula = Effect?.Internal?.Impact?.ToString() ?? string.Empty;
            DiceValuePopupResult result = await ShowPopupAsync<DiceValuePopup, DiceValuePopupResult, DiceValuePopupParameters>(
                new DiceValuePopupParameters { Formula = formula }
            );

            if (result != null)
            {
                ResetAdjustment();
                Effect.Impact = result.DicePool;
                _dicePoolToReset = null;
            }

            CostMonitor.UpdateCost();
        }


        private DicePool _dicePoolToReset = null;

        private double _adjustment = 0;
        public double Adjustment
        {
            get => _adjustment;
            set
            {
                if (value < 1 && value > -1)
                {
                    if (_dicePoolToReset != null)
                    {
                        Effect.Impact = _dicePoolToReset;
                        _dicePoolToReset = null;
                        CostMonitor.UpdateCost();
                    }
                }
                else
                {
                    bool crossInteger = Math.Abs(Math.Floor(_adjustment) - Math.Floor(value)) >= 1;

                    if (crossInteger || value == -5 || value == 5)
                    {
                        int adjustmentPercent = (int)(value > 0 ? Math.Floor(value) : Math.Ceiling(value));
                        Adjust(adjustmentPercent * 10);
                    }
                }

                SetProperty(ref _adjustment, value);
            }
        }

        private void Adjust(int percent)
        {
            _dicePoolToReset = _dicePoolToReset == null ? Effect.Impact.Copy() : _dicePoolToReset;
            Effect.Impact = DicePool.FromAdjusted(_dicePoolToReset, percent);
            CostMonitor.UpdateCost();
        }

        [RelayCommand]
        private void ApplyAdjustment()
        {
            _dicePoolToReset = null;
            Adjustment = 0;
        }

        [RelayCommand]
        private void ResetAdjustment()
        {
            Adjustment = 0;
            CostMonitor.UpdateCost();
        }
    }
}
