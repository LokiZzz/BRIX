using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class AspectPanelVM : ViewModelBase
    {
        private readonly EffectModelBase? _aspectOwnerEffect;
        private readonly AbilityCostMonitorPanelVM _costMonitor;

        private readonly EAspectScope _scope;

        public AspectPanelVM(AbilityCostMonitorPanelVM costMonitor, EffectModelBase effect)
        {
            _scope = EAspectScope.Effect;

            if(costMonitor?.Ability != null && !costMonitor.Ability.Effects.Contains(effect))
            {
                throw new ArgumentException(
                    "Инициализируя модель аспекта, необходимо передавать способность и её (!!!) эффект."
                );
            }

            _aspectOwnerEffect = effect;
            _costMonitor = costMonitor ?? throw new Exception("Передан не инициализированный CostMonitor.");
            InitializeAspects();
            UpdateVisibilityProperties();
        }

        public AspectPanelVM(AbilityCostMonitorPanelVM costMonitor)
        {
            _scope = EAspectScope.Ability;
            _costMonitor = costMonitor ?? throw new Exception("Передан не инициализированный CostMonitor.");

            if (costMonitor.Ability == null)
            {
                throw new Exception("Передан не инициализированный CostMonitor, Ability == null.");
            }

            InitializeAspects();
            UpdateVisibilityProperties();
        }

        public void InitializeAspects()
        {
            if (_scope == EAspectScope.Ability)
            {
                List<AspectUtilityModel> aspects = _costMonitor.Ability.ConcordedAspects
                    .Select(GetAspectModel)
                    .Where(x => x != null)
                    .ToList();
                AspectsCollection = new ObservableCollection<AspectUtilityModel>(aspects);
            }
            else if(_scope == EAspectScope.Effect && _aspectOwnerEffect != null)
            {
                List<AspectUtilityModel> aspects = _aspectOwnerEffect.Aspects
                    .Select(GetAspectModel)
                    .Where(x => x != null)
                    .ToList();
                AspectsCollection = new ObservableCollection<AspectUtilityModel>(aspects);
            }

            if (AspectsCollection.Any())
            {
                SelectedAspect = AspectsCollection.First();
            }
        }

        private ObservableCollection<AspectUtilityModel> _aspectsCollection = [];
        public ObservableCollection<AspectUtilityModel> AspectsCollection
        {
            get => _aspectsCollection;
            set
            {
                SetProperty(ref _aspectsCollection, value);
                UpdateVisibilityProperties();
            }
        }

        private AspectUtilityModel _selectedAspect = new();
        public AspectUtilityModel SelectedAspect
        {
            get => _selectedAspect;
            set
            {
                SetProperty(ref _selectedAspect, value);
                UpdateVisibilityProperties();
            }
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

            if (_scope == EAspectScope.Effect && _aspectOwnerEffect != null)
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
            else if(_scope == EAspectScope.Ability)
            {
                AspectModelBase aspectToEdit = _costMonitor.Ability.ConcordedAspects
                    .First(x => x.InternalModel.GetType().Equals(SelectedAspect.LibraryAspectType));
                await Navigation.NavigateAsync(
                    SelectedAspect.EditPage.Name,
                    Services.ENavigationMode.Push,
                    (NavigationParameters.CostMonitor, _costMonitor.Copy()),
                    (NavigationParameters.Aspect, aspectToEdit.Copy())
                );
            }
        }

        [RelayCommand]
        public async Task ConcordSelectedAspect()
        {
            if(_scope == EAspectScope.Ability || _aspectOwnerEffect == null)
            {
                throw new Exception(
                    $"Согласовать аспект можно только из эффекта."
                );
            }

            AlertPopupResult? result = await Ask(Localization.AskIfYouWantToConcord);

            if (result?.Answer == EAlertPopupResult.Yes)
            { // rename: aspectToConcord
                AspectModelBase aspectToEdit = _aspectOwnerEffect.Aspects
                    .First(x => x.InternalModel.GetType().Equals(SelectedAspect.LibraryAspectType));
                _costMonitor.Ability.Concord(aspectToEdit);
                UpdateVisibilityProperties();
            }
        }

        [RelayCommand]
        public void DiscordSelectedAspect()
        {
            AspectModelBase aspectToEdit = _scope switch
            {
                EAspectScope.Effect => _aspectOwnerEffect?.Aspects
                    .First(x => x.InternalModel.GetType().Equals(SelectedAspect.LibraryAspectType))
                    ?? throw new Exception("Эффект аспекта не определён."),
                EAspectScope.Ability => _costMonitor.Ability.ConcordedAspects
                    .First(x => x.InternalModel.GetType().Equals(SelectedAspect.LibraryAspectType)),
                _ => throw new Exception("Аспект для рассогласования не определён.")
            };

            _costMonitor.Ability.Discord(aspectToEdit);
            InitializeAspects();
            UpdateVisibilityProperties();
        }

        public void UpdateAspect(AspectModelBase aspect)
        {
            AspectUtilityModel aspectToUpdate = AspectsCollection
                .Single(x => x.LibraryAspectType == aspect.InternalModel.GetType());

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
                    model.ConcreteAspect = aspect;
                }

            }

            return model ?? throw new Exception($"В AspectsDictionary не найдена модель для {aspect?.GetType()}");
        }

        #region Visibility

        public bool ShowEdit => _scope == EAspectScope.Ability
            || SelectedAspect.ConcreteAspect?.IsConcorded == false;
        public bool ShowConcord => SelectedAspect.ConcreteAspect?.IsConcorded == false;
        public bool ShowDiscord => SelectedAspect.ConcreteAspect?.IsConcorded == true && ShowEdit;
        public bool ShowBigDiscord => SelectedAspect.ConcreteAspect?.IsConcorded == true && !ShowEdit;
        public bool ShowPanel => AspectsCollection.Any();

        public void UpdateVisibilityProperties()
        {
            OnPropertyChanged(nameof(ShowEdit));
            OnPropertyChanged(nameof(ShowConcord));
            OnPropertyChanged(nameof(ShowDiscord));
            OnPropertyChanged(nameof(ShowBigDiscord));
            OnPropertyChanged(nameof(ShowPanel));
        }

        #endregion
    }

    public enum EAspectScope
    {
        Effect = 0,
        Ability = 1
    }
}
