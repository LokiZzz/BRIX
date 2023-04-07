using BRIX.Library.DiceValue;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class HealDamageEffectPageVM : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private EEditingMode _mode;

        [ObservableProperty]
        private DamageEffectModel _damage = new();

        [ObservableProperty]
        private AspectPanelViewModel _aspects;

        [ObservableProperty]
        private AbilityCostMonitorPanelVM _costMonitor;

        [RelayCommand]
        private async Task EditFormula()
        {
            string formula = Damage?.Internal?.Impact?.ToString() ?? string.Empty;
            DiceValuePopupResult result = await ShowPopupAsync<DiceValuePopup, DiceValuePopupResult, DiceValuePopupParameters>(
                new DiceValuePopupParameters { Formula = formula }
            );

            if (result != null)
            {
                ResetAdjustment();
                Damage.Impact = result.DicePool;
                _dicePoolToReset = null;
            }

            CostMonitor.UpdateCost();
        }

        [RelayCommand]
        private async Task Save()
        {
            switch(Mode)
            {
                case EEditingMode.Add:
                    await Navigation.Back(stepsBack: 2, 
                        (NavigationParameters.Effect, Damage),
                        (NavigationParameters.EditMode, Mode)
                    );
                    break;
                case EEditingMode.Edit:
                case EEditingMode.Upgrade:
                    await Navigation.Back(stepsBack: 1,
                        (NavigationParameters.Effect, Damage),
                        (NavigationParameters.EditMode, Mode)
                    );
                    break;
            }
        }

        private bool _alreadyInitialized = false;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (_alreadyInitialized)
            {
                HandleBackFromEditingAspect(query);
            }
            else
            {
                HandleInitial(query);
            }

            CostMonitor.SaveCommand = SaveCommand;
            CostMonitor.UpdateCost();

            query.Clear();
        }

        private void HandleBackFromEditingAspect(IDictionary<string, object> query)
        {
            AspectModelBase aspect = query.GetParameterOrDefault<AspectModelBase>(NavigationParameters.Aspect);

            if (aspect != null)
            {
                Damage.UpdateAspect(aspect);
                Aspects.UpdateAspect(aspect);
            }

            CostMonitor.UpdateCost();
        }

        private void HandleInitial(IDictionary<string, object> query)
        {
            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            CostMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);
            Damage = query.GetParameterOrDefault<DamageEffectModel>(NavigationParameters.Effect) ?? new();

            switch (Mode)
            {
                case EEditingMode.Add:
                    Damage.Impact = new DicePool((1, 4));
                    CostMonitor.Ability.AddEffect(Damage);
                    break;
                case EEditingMode.Edit:
                case EEditingMode.Upgrade:
                    CostMonitor.Ability.UpdateEffect(Damage);
                    break;
            }

            Aspects = new AspectPanelViewModel(CostMonitor, Damage);

            _alreadyInitialized = true;
        }

        #region Fast adjustment

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
                        Damage.Impact = _dicePoolToReset;
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
            _dicePoolToReset = _dicePoolToReset == null ? Damage.Impact.Copy() : _dicePoolToReset;
            Damage.Impact = DicePool.FromAdjusted(_dicePoolToReset, percent);
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

        #endregion
    }
}
