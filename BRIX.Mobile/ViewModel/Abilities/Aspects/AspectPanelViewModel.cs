using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class AspectPanelViewModel : ViewModelBase
    {
        private EffectModelBase _aspectOwnerEffect;
        private AbilityCostMonitorPanelVM _costMonitor;

        public AspectPanelViewModel(AbilityCostMonitorPanelVM costMonitor, EffectModelBase effect)
        {
            if(costMonitor?.Ability != null && !costMonitor.Ability.Effects.Contains(effect))
            {
                throw new ArgumentException(
                    "Инициализируя модель аспекта, необходимо передавать способность и её (!!!) эффект."
                );
            }

            _aspectOwnerEffect = effect;
            _costMonitor = costMonitor;

            AspectsCollection = GetAspects(effect);

            if (AspectsCollection.Any())
            {
                SelectedAspect = AspectsCollection.First();
            }
        }

        private ObservableCollection<AspectUtilityModel> _aspectsCollection = new();
        public ObservableCollection<AspectUtilityModel> AspectsCollection
        {
            get => _aspectsCollection;
            set => SetProperty(ref _aspectsCollection, value);
        }

        private AspectUtilityModel _selectedAspect = new();
        public AspectUtilityModel SelectedAspect
        {
            get => _selectedAspect;
            set => SetProperty(ref _selectedAspect, value);
        }

        [RelayCommand]
        public async Task NavigateToAspect()
        {
            AspectModelBase aspectToEdit = _aspectOwnerEffect.GetAspect(SelectedAspect.LibraryAspectType);

            await Navigation.NavigateAsync(
                SelectedAspect.EditPage.Name,
                Services.ENavigationMode.Push,
                (NavigationParameters.CostMonitor, _costMonitor.Copy()),
                (NavigationParameters.Effect, _aspectOwnerEffect.Copy()),
                (NavigationParameters.Aspect, aspectToEdit.Copy())
            );
        }

        public void UpdateAspect(AspectModelBase aspect)
        {
            AspectUtilityModel aspectToUpdate = AspectsCollection
                .Single(x => x.LibraryAspectType == aspect.InternalBase.GetType());

            if (aspectToUpdate != null)
            {
                int index = AspectsCollection.IndexOf(aspectToUpdate);
                AspectUtilityModel newAspectUtilityModel = GetAspectModel(aspect);
                AspectsCollection[index] = newAspectUtilityModel;
                SelectedAspect = newAspectUtilityModel;
            }
        }

        private ObservableCollection<AspectUtilityModel> GetAspects(EffectModelBase effect)
        {
            List<AspectUtilityModel> aspects = effect.Aspects
                .Select(GetAspectModel)
                .Where(x => x != null)
                .ToList();

            return new ObservableCollection<AspectUtilityModel>(aspects);
        }

        private AspectUtilityModel GetAspectModel(AspectModelBase aspect)
        {
            AspectUtilityModel? model = null;

            if (aspect != null)
            {
                AspectsDictionary.Collection.TryGetValue(aspect.GetType(), out model);

                if (model != null)
                {
                    model.Description = aspect.Description;
                }
            }

            return model ?? throw new Exception($"В AspectsDictionary не найдена модель для {aspect?.GetType()}");
        }
    }
}
