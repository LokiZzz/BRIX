using BRIX.Library.Aspects;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class AspectPanelViewModel : ViewModelBase
    {
        public AspectPanelViewModel(EffectModelBase effect)
        {
            AspectsCollection = GetAspects(effect);
            SelectedAspect = AspectsCollection.First();
        }

        [ObservableProperty]
        private ObservableCollection<AspectUtilityModel> _aspectsCollection = new();

        [ObservableProperty]
        private AspectUtilityModel _selectedAspect = new();

        private ObservableCollection<AspectUtilityModel> GetAspects(EffectModelBase effect)
        {
            List<AspectUtilityModel> aspects = effect.Aspects.Select(GetAspectModel).Where(x => x != null).ToList();

            return new ObservableCollection<AspectUtilityModel>(aspects);
        }

        private AspectUtilityModel GetAspectModel(AspectModelBase aspect)
        {
            AspectsDictionary.Collection.TryGetValue(aspect.GetType(), out AspectUtilityModel model);

            return model;
        }
    }
}
