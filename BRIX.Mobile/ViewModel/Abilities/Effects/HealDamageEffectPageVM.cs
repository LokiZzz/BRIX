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

            _dicePoolToReset = _dicePoolToReset == null ? Damage.Impact.Copy() : _dicePoolToReset;
            Damage.Impact = DicePool.FromAdjusted(_dicePoolToReset, percent);
            Ability.UpdateCost();
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
            Damage.Impact = _dicePoolToReset;
            _dicePoolToReset = null;
            _doNotAdjustOnce = true;
            Adjustment = 0;
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
            // на этой страничке. Поэтому отдельно пришедшие в эту страничку копии объектов объединяются по новой в
            // зависимости от режима работы. Эта страница своим результатом отдаёт исключительно эффект, при чём в
            // отстранении от способности. Как распорядится этим эффектом решает страница, вызвавшая эту.

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
