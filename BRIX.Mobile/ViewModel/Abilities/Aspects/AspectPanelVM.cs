using BRIX.Mobile.Models.Abilities;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class AspectPanelVM : ViewModelBase
    {
        private readonly EffectModelBase? _aspectOwnerEffect;
        private readonly AbilityCostMonitorPanelVM _costMonitor;

        public AspectPanelVM(AbilityCostMonitorPanelVM costMonitor, EffectModelBase effect)
        {
            if(costMonitor?.Ability != null && !costMonitor.Ability.Effects.Contains(effect))
            {
                throw new ArgumentException(
                    "Инициализируя модель аспекта, необходимо передавать способность и её (!!!) эффект."
                );
            }

            _aspectOwnerEffect = effect;
            _costMonitor = costMonitor ?? throw new Exception("Передан не инициализированный CostMonitor.");

            List<AspectUtilityModel> aspects = effect.Aspects
                .Select(GetAspectModel)
                .Where(x => x != null)
                .ToList();
            AspectsCollection = new ObservableCollection<AspectUtilityModel>(aspects);

            if (AspectsCollection.Any())
            {
                SelectedAspect = AspectsCollection.First();
            }
        }

        public AspectPanelVM(AbilityCostMonitorPanelVM costMonitor)
        {
            _costMonitor = costMonitor ?? throw new Exception("Передан не инициализированный CostMonitor.");

            if(costMonitor.Ability == null)
            {
                throw new Exception("Передан не инициализированный CostMonitor, Ability == null.");
            }

            List<AspectUtilityModel> aspects = costMonitor.Ability.ConcordedAspects
                .Select(GetAspectModel)
                .Where(x => x != null)
                .ToList();
            AspectsCollection = new ObservableCollection<AspectUtilityModel>(aspects);

            if (AspectsCollection.Any())
            {
                SelectedAspect = AspectsCollection.First();
            }
        }

        private ObservableCollection<AspectUtilityModel> _aspectsCollection = [];
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
            if(SelectedAspect.LibraryAspectType == null)
            {
                throw new ArgumentNullException(nameof(SelectedAspect.LibraryAspectType));
            }

            if (SelectedAspect.EditPage == null)
            {
                throw new ArgumentNullException(nameof(SelectedAspect.EditPage));
            }

            if (_aspectOwnerEffect != null)
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
            else
            {
                AspectModelBase aspectToEdit = _costMonitor.Ability.ConcordedAspects
                    .First(x => x.GetType().Equals(SelectedAspect.LibraryAspectType));
                await Navigation.NavigateAsync(
                    SelectedAspect.EditPage.Name,
                    Services.ENavigationMode.Push,
                    (NavigationParameters.CostMonitor, _costMonitor.Copy()),
                    (NavigationParameters.Aspect, aspectToEdit.Copy())
                );
            }
        }

        public void UpdateAspect(AspectModelBase aspect)
        {
            AspectUtilityModel aspectToUpdate = AspectsCollection
                .Single(x => x.LibraryAspectType == aspect.Internal.GetType());

            if (aspectToUpdate != null)
            {
                int index = AspectsCollection.IndexOf(aspectToUpdate);
                AspectUtilityModel newAspectUtilityModel = GetAspectModel(aspect);
                AspectsCollection[index] = newAspectUtilityModel;
                SelectedAspect = newAspectUtilityModel;
            }
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
