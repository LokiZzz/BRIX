using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public abstract partial class AspectPageVMBase<T> : ViewModelBase, IQueryAttributable where T : AspectModelBase
    {
        [ObservableProperty]
        private AbilityCostMonitorPanelVM _costMonitor;

        [ObservableProperty]
        private EffectModelBase _effect;

        [ObservableProperty]
        private T _aspect;

        [RelayCommand]
        private async Task Save()
        {
            await Navigation.Back(stepsBack: 1, (NavigationParameters.Aspect, Aspect));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CostMonitor = query.GetParameterOrDefault<AbilityCostMonitorPanelVM>(NavigationParameters.CostMonitor);
            CostMonitor.SaveCommand = SaveCommand;
            Effect = query.GetParameterOrDefault<DamageEffectModel>(NavigationParameters.Effect);
            Aspect = query.GetParameterOrDefault<T>(NavigationParameters.Aspect);
            Aspect.CostMonitor = CostMonitor;

            Initialize();

            Effect.UpdateAspect(Aspect);
            CostMonitor.Ability.UpdateEffect(Effect);

            query.Clear();
        }

        /// <summary>
        /// Метод для дополнительной инициализации деталей интерфейса.
        /// </summary>
        public virtual void Initialize() { }
    }
}
