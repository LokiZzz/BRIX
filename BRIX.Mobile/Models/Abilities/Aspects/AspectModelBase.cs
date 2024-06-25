using BRIX.Library.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public abstract partial class AspectModelBase : ObservableObject 
    {
        public AspectModelBase(AspectBase model)
        {
            Internal = model;
        }

        public AspectBase Internal { get; set; }

        public AbilityCostMonitorPanelVM CostMonitor { get; set; } = new();

        public string Name => AspectsDictionary.Collection[GetType()].Name;

        public string Description => Internal == null ? string.Empty : Internal.ToLexis();

        public void UpdateCost() => CostMonitor?.UpdateCost();

        protected new bool SetProperty<TModel, T>(
            T oldValue, 
            T newValue, 
            TModel model, 
            Action<TModel, T> callback, 
            [CallerMemberName] string? propertyName = null) where TModel : class
        {
            bool set = base.SetProperty(oldValue, newValue, model, callback, propertyName);
            UpdateCost();

            return set;
        }
    }
}
