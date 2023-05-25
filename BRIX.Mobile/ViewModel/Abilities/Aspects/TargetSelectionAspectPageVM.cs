using BRIX.Library.Aspects.TargetSelection;
using BRIX.Library.Enums;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class TargetSelectionAspectPageVM : AspectPageVMBase<TargetSelectionAspectModel>
    {
        public TargetSelectionAspectPageVM(ILocalizationResourceManager localization)
        {
            Localization = localization;
        }

        public ILocalizationResourceManager Localization { get; }

        public override void Initialize()
        {
            if (Aspect.Internal.Strategy == ETargetSelectionStrategy.NTargetsAtDistanсeL)
            {
                SetNTAD();
            }
            else
            {
                SetAREA();
            }

            SetShape(Aspect.AreaType.ToString("G"));

            Restrictions = new(Aspect.Internal.TargetSelectionRestrictions.Conditions.Select(ToRestrictionsVM));
            OnPropertyChanged(nameof(ShowNoRestrictionsText));
            Sizes = new(Aspect.Internal.TargetsSizes.AllowedTargetSizes.Select(x => new TargetSizeVM
            {
                Size = x,
                Text = Localization[x.ToString("G")].ToString()
            }));
        }

        #region Strategy

        [RelayCommand]
        public void SetNTAD()
        {
            IsAREA = false;
            IsNTAD = true;
            IsCharacter = false;
            Aspect.Strategy = ETargetSelectionStrategy.NTargetsAtDistanсeL;
        }

        [RelayCommand]
        public void SetAREA()
        {
            IsNTAD = false;
            IsAREA = true;
            IsCharacter = false;
            Aspect.Strategy = ETargetSelectionStrategy.Area;
        }

        [RelayCommand]
        public void SetCharacter()
        {
            IsNTAD = false;
            IsAREA = false;
            IsCharacter = true;
            Aspect.Strategy = ETargetSelectionStrategy.CharacterHimself;
        }

        private bool _isNTAD = true;
        public bool IsNTAD
        {
            get => _isNTAD;
            set
            {
                SetProperty(ref _isNTAD, value);
                OnPropertyChanged(nameof(ShowNTADAndAREASettings));
            }
        }


        private bool _isAREA = true;
        public bool IsAREA
        {
            get => _isAREA;
            set
            {
                SetProperty(ref _isAREA, value);
                OnPropertyChanged(nameof(ShowNTADAndAREASettings));
            }
        }

        private bool _isCharacter = true;
        public bool IsCharacter
        {
            get => _isCharacter;
            set
            {
                SetProperty(ref _isCharacter, value);
                OnPropertyChanged(nameof(ShowNTADAndAREASettings));
            }
        }

        public bool ShowNTADAndAREASettings => IsNTAD || IsAREA;

        #endregion

        #region Targets selection restrictions

        private ObservableCollection<TargetSelectionRestrictionPropertyVM> _restrictions;
        public ObservableCollection<TargetSelectionRestrictionPropertyVM> Restrictions
        {
            get => _restrictions;
            set => SetProperty(ref _restrictions, value);
        }

        public bool ShowNoRestrictionsText => Restrictions?.Any() == false;

        [RelayCommand]
        public async Task AddRestriction()
        {
            List<ETargetSelectionRestrictions> customRestrictions = new()
            {
                ETargetSelectionRestrictions.LowRarityProperty,
                ETargetSelectionRestrictions.MediumRarityProperty,
                ETargetSelectionRestrictions.HighRarityProperty,
            };

            List<object> allRestrictions = Enum.GetValues<ETargetSelectionRestrictions>()
                .Select(x => new TargetSelectionRestrictionPropertyVM { 
                    Restriction = x, 
                    Text = Localization[x.ToString("G")].ToString() 
                })
                .Where(x => !Restrictions.Any(y => y.Restriction == x.Restriction) 
                    || customRestrictions.Any(y => y == x.Restriction))
                .Select(x => x as object)
                .ToList();

            PickerPopupParameters parameters = new()
            {
                Title = Resources.Localizations.Localization.TargetThreeDot,
                Items = allRestrictions,
            };
            PickerPopupResult result = await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(parameters);

            if (result != null)
            {
                TargetSelectionRestrictionPropertyVM concreteResult = result.SelectedItem
                    as TargetSelectionRestrictionPropertyVM;

                if(customRestrictions.Any(x => x ==  concreteResult.Restriction))
                {
                    string message = GetCustomRestrictionHint(concreteResult.Restriction);

                    EntryPopupResult entryResult = await ShowPopupAsync<EntryPopup, EntryPopupResult, EntryPopupParameters>(
                        new EntryPopupParameters
                        {
                            Title = Resources.Localizations.Localization.TargetSelectionRestriction,
                            Message = message,
                            Placeholder = Resources.Localizations.Localization.TargetProperty,
                            ButtonText = Resources.Localizations.Localization.Ok,
                        }
                    );

                    if(entryResult == null)
                    {
                        return;
                    }

                    concreteResult.Text = entryResult.Text;
                }

                if (Restrictions.Any(x => x.Restriction == concreteResult.Restriction && x.Text == concreteResult.Text))
                {
                    await ShowPopupAsync<AlertPopup, AlertPopupResult, AlertPopupParameters>(new AlertPopupParameters
                    {
                        Title = Resources.Localizations.Localization.Warning,
                        Message = Resources.Localizations.Localization.TargetSelectionRestrictionWarning,
                        OkText = Resources.Localizations.Localization.Ok,
                        Mode = EAlertMode.ShowMessage
                    });

                    return;
                }

                Restrictions.Add(concreteResult);
                Aspect.Internal.TargetSelectionRestrictions.Conditions.Add((concreteResult.Restriction, concreteResult.Text));
                OnPropertyChanged(nameof(ShowNoRestrictionsText));
            }

            CostMonitor.UpdateCost();
        } 

        [RelayCommand]
        public void DeleteRestriction(TargetSelectionRestrictionPropertyVM property)
        {
            Restrictions.Remove(property);

            (ETargetSelectionRestrictions Type, string Comment) conditionToDelete = 
                Aspect.Internal.TargetSelectionRestrictions.Conditions.FirstOrDefault(x => 
                    x.Type == property.Restriction || x.Comment == property.Text);
            Aspect.Internal.TargetSelectionRestrictions.Conditions.Remove(conditionToDelete);

            CostMonitor.UpdateCost();
            OnPropertyChanged(nameof(ShowNoRestrictionsText));
        }

        private TargetSelectionRestrictionPropertyVM ToRestrictionsVM((ETargetSelectionRestrictions Type, string Comment) restriction)
        {
            TargetSelectionRestrictionPropertyVM restrictionVM = new() { Restriction = restriction.Type };

            switch(restriction.Type)
            {
                case ETargetSelectionRestrictions.LowRarityProperty:
                case ETargetSelectionRestrictions.MediumRarityProperty:
                case ETargetSelectionRestrictions.HighRarityProperty:
                    restrictionVM.Text = restriction.Comment; 
                    break;
                default:
                    restrictionVM.Text = Localization[restriction.Type.ToString("G")].ToString();
                    break;
            }

            return restrictionVM;
        }

        private string GetCustomRestrictionHint(ETargetSelectionRestrictions restriction)
        {
            switch (restriction)
            {
                case ETargetSelectionRestrictions.LowRarityProperty:
                    return Resources.Localizations.Localization.EnterLowRarityRestriction;
                case ETargetSelectionRestrictions.MediumRarityProperty:
                    return Resources.Localizations.Localization.EnterMediumRarityRestriction;
                case ETargetSelectionRestrictions.HighRarityProperty:
                    return Resources.Localizations.Localization.EnterHighRarityRestriction;
                default:
                    return null;
            }
        }

        #endregion

        #region Target size

        private ObservableCollection<TargetSizeVM> _sizes;
        public ObservableCollection<TargetSizeVM> Sizes
        {
            get => _sizes;
            set => SetProperty(ref _sizes, value);
        }

        [RelayCommand]
        public async Task AddSize()
        {
            List<object> allSizes = Enum.GetValues<ETargetSize>()
                .Select(x => new TargetSizeVM
                {
                    Size = x,
                    Text = Localization[x.ToString("G")].ToString()
                })
                .Where(x => !Sizes.Any(y => y.Size == x.Size))
                .Select(x => x as object)
                .ToList();

            PickerPopupParameters parameters = new()
            {
                Title = Resources.Localizations.Localization.TargetsSizes,
                Items = allSizes,
                SelectedItems = new List<object> { allSizes.First() },
                SelectMultiple = true
            };
            PickerPopupResult result = await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(parameters);

            if (result != null)
            {
                List<TargetSizeVM> concreteResult = result.SelectedItems.Select(x => x as TargetSizeVM).ToList();

                if (Sizes.Any(x => concreteResult.Any(y => y.Size == x.Size)))
                {
                    return;
                }

                foreach(TargetSizeVM sizeVM in concreteResult)
                {
                    Sizes.Add(sizeVM); 
                    Aspect.Internal.TargetsSizes.AddSize(sizeVM.Size);
                }

                Sizes = new(Sizes.OrderBy(x => x.Size));
            }

            CostMonitor.UpdateCost();
        }

        [RelayCommand]
        public void DeleteSize(TargetSizeVM property)
        {
            Sizes.Remove(property);
            Aspect.Internal.TargetsSizes.RemoveSize(property.Size);

            CostMonitor.UpdateCost();
        }

        #endregion

        #region Area shape

        [RelayCommand]
        public void SetShape(string shape)
        {
            Enum.TryParse(shape, out EAreaType parsedShape);
            Shape = parsedShape;
            Aspect.AreaType = parsedShape;
        }

        private EAreaType _shape;
        public EAreaType Shape
        {
            get => _shape;
            set
            {
                _shape = value;
                OnPropertyChanged(nameof(IsBrick));
                OnPropertyChanged(nameof(IsCone));
                OnPropertyChanged(nameof(IsCylinder));
                OnPropertyChanged(nameof(IsSphere));
                OnPropertyChanged(nameof(IsArbitrary));
            }
        }

        public bool IsBrick => Shape == EAreaType.Brick;
        public bool IsCone => Shape == EAreaType.Cone;
        public bool IsCylinder => Shape == EAreaType.Cylinder;
        public bool IsSphere => Shape == EAreaType.Sphere;
        public bool IsArbitrary => Shape == EAreaType.Arbitrary;

        #endregion
    }

    public class TargetSelectionRestrictionPropertyVM
    {
        public ETargetSelectionRestrictions Restriction { get; set; }
        public string Text { get; set; }

        public override string ToString() => Text;
    }

    public class TargetSizeVM
    {
        public ETargetSize Size { get; set; }
        public string Text { get; set; }

        public override string ToString() => Text;
    }
}
