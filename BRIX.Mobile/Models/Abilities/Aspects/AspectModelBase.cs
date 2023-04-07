using BRIX.Lexica;
using BRIX.Library.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Abilities;
using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.ViewModel.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public abstract partial class AspectModelBase : ObservableObject 
    {
        public AspectModelBase(AspectBase model)
        {
            InternalBase = model;
        }

        public AspectBase InternalBase { get; set; }

        public AbilityCostMonitorPanelVM CostMonitor { get; set; }

        public T GetSpecificAspect<T>() where T : AspectBase
        {
            return InternalBase as T;
        }

        public string Name => AspectsDictionary.Collection[GetType()].Name;

        public string Description 
        {
            get
            {
                ILocalizationResourceManager localization = ServicePool.GetService<ILocalizationResourceManager>();
                ELexisLanguage language = localization.LexisLanguage;

                return AspectLexis.GetAspectDescription(InternalBase, language);
            } 
        }

        public void UpdateCost() => CostMonitor?.UpdateCost();
    }
}
