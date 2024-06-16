using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class EffectPageVMBase<T> : ViewModelBase, IQueryAttributable where T : EffectModelBase, new()
    {
        private EEditingMode _mode;
        public EEditingMode Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
        }

        private T? _effect;
        public T? Effect
        {
            get => _effect;
            set
            {
                SetProperty(ref _effect, value);

                if (_effect != null)
                {
                    _effect.EffectPropertyChanged += UpdateCost;
                }
            }
        }

        private void UpdateCost(object? sender, EventArgs e)
        {
            CostMonitor?.UpdateCost();
        }

        private AspectPanelViewModel? _aspects;
        public AspectPanelViewModel? Aspects
        {
            get => _aspects;
            set => SetProperty(ref _aspects, value);
        }

        private AbilityCostMonitorPanelVM? _costMonitor;
        public AbilityCostMonitorPanelVM? CostMonitor
        {
            get => _costMonitor;
            set => SetProperty(ref _costMonitor, value);
        }

        [RelayCommand]
        private async Task Save()
        {
            switch (Mode)
            {
                case EEditingMode.Add:
                    await Navigation.Back(stepsBack: 2,
                        (NavigationParameters.Effect, Effect),
                        (NavigationParameters.EditMode, Mode)
                    );
                    break;
                case EEditingMode.Edit:
                    await Navigation.Back(stepsBack: 1,
                        (NavigationParameters.Effect, Effect),
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

            if (CostMonitor != null)
            {
                CostMonitor.SaveCommand = SaveCommand;
                CostMonitor.UpdateCost();
            }

            query.Clear();
        }

        private void HandleBackFromEditingAspect(IDictionary<string, object> query)
        {
            AspectModelBase? aspect = query.GetParameterOrDefault<AspectModelBase>(NavigationParameters.Aspect);

            if (aspect != null)
            {
                if (Effect != null && Aspects != null)
                {
                    Effect.UpdateAspect(aspect);
                    Aspects.UpdateAspect(aspect);
                }
                else
                {
                    throw new Exception("Эффект или его аспекты не инициализированы.");
                }
            }

            CostMonitor?.UpdateCost();
        }

        protected virtual void HandleInitial(IDictionary<string, object> query)
        {
            Mode = query.GetParameterOrDefault<EEditingMode>(NavigationParameters.EditMode);
            Effect = query.GetParameterOrDefault<T>(NavigationParameters.Effect) ?? new T();
            CostMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);

            if (CostMonitor?.Ability != null && CostMonitor?.ShowCost == true)
            {
                switch (Mode)
                {
                    case EEditingMode.Add:
                        CostMonitor.Ability.AddEffect(Effect);
                        break;
                    case EEditingMode.Edit:
                        CostMonitor.Ability.UpdateEffect(Effect);
                        break;
                }
            }

            if (CostMonitor != null)
            {
                Aspects = new AspectPanelViewModel(CostMonitor, Effect);
            }

            _alreadyInitialized = true;
        }
    }
}
