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

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class HealDamageEffectPageVM : ViewModelBase, IQueryAttributable
    {
        [ObservableProperty]
        private EEditingMode _mode;

        [ObservableProperty]
        private DamageEffectModel _damage = new();

        [ObservableProperty]
        private AbilityModel _ability = new();

        [RelayCommand]
        private async Task EditFormula()
        {
            string formula = Damage?.Internal?.Impact?.ToString() ?? string.Empty;
            DiceValuePopupResult result = await ShowPopupAsync<DiceValuePopup, DiceValuePopupResult, DiceValuePopupParameters>(
                new DiceValuePopupParameters { Formula = formula }
            );

            if (result != null)
            {
                Damage.Impact = result.DicePool;
                _dicePoolToReset = null;
            }

            Ability.UpdateCost();
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
                        Damage.Impact = _dicePoolToReset;
                        _dicePoolToReset = null;
                    }
                }
                else
                {
                    bool crossInteger = Math.Abs(Math.Floor(_adjustment) - Math.Floor(value)) >= 1;

                    if (crossInteger || value == -5 || value == 5)
                    {
                        SetProperty(ref _adjustment, value);
                        Adjust(value.Round() * 10);
                    }
                }

                _adjustment = value;
                OnPropertyChanged();
            }
        }

        private void Adjust(int percent)
        {
            _dicePoolToReset = _dicePoolToReset == null ? Damage.Impact.Copy() : _dicePoolToReset;
            Damage.Impact = DicePool.FromAdjusted(_dicePoolToReset, percent);
            Ability.UpdateCost();
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
            Ability.UpdateCost();
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

        public override Task OnNavigatedAsync()
        {
            Ability.UpdateCost();

            return Task.CompletedTask;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Приходят копии способности и объекта, никак не связанны с экземплярами, существовавшими во вью-модели
            // страницы, которая вызвала эту. При этом способность и эффект здесь никак не связаны.

            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            Ability = query.GetParameterOrDefault<AbilityModel>(NavigationParameters.Ability) ?? new();
            Damage = query.GetParameterOrDefault<DamageEffectModel>(NavigationParameters.Effect) ?? new();

            // Экземпляр способности здесь существует только для того, чтобы проиллюстрировать изменение её стоимости
            // на этой страничке. Поэтому отдельно пришедшие в эту страничку копии способности и эффекта объединяются
            // по новой в зависимости от режима работы. При этом отдаст эта страница исключительно эффект.

            switch (Mode)
            {
                case EEditingMode.Add:
                    Ability.AddEffect(Damage);
                    break;
                case EEditingMode.Edit:
                case EEditingMode.Upgrade:
                    Ability.UpdateEffect(Damage);
                    break;
            }

            if (Damage.Impact.IsEmpty)
            {
                Damage.Impact = new DicePool((1, 4));
            }

            query.Clear();
        }
    }
}
