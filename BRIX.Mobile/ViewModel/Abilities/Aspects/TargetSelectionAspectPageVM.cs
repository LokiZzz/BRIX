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
    /// <summary>
    /// Просит рефакторинга, но не срочно, нужно убрать из регионов в TargetSelectionAspectModel по максимуму.
    /// </summary>
    public partial class TargetSelectionAspectPageVM(ILocalizationResourceManager localization) 
        : AspectPageVMBase<TargetSelectionAspectModel>
    {
        public ILocalizationResourceManager Localization { get; } = localization;

        public override void Initialize()
        {
            if(Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            if (Aspect.Internal.Strategy == ETargetSelectionStrategy.NTargetsAtDistanсeL)
            {
                SetNTAD();
            }
            else
            {
                SetAREA();
            }

            SetShape(Aspect.AreaShape.AreaType.ToString("G"));

            Sizes = new(Aspect.Internal.TargetsSizes.AllowedTargetSizes.Select(x => new TargetSizeVM
            {
                Size = x,
                Text = Localization[x.ToString("G")].ToString() ?? string.Empty
            }));
            OnPropertyChanged(nameof(ShowSizesCollection));

            Aspect.AreaShape.CostMonitor = Aspect.CostMonitor;
        }

        #region Strategy

        [RelayCommand]
        public void SetNTAD()
        {
            if (Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            IsAREA = false;
            IsNTAD = true;
            IsCharacter = false;
            Aspect.Strategy = ETargetSelectionStrategy.NTargetsAtDistanсeL;
        }

        [RelayCommand]
        public void SetAREA()
        {
            if (Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            IsNTAD = false;
            IsAREA = true;
            IsCharacter = false;
            Aspect.Strategy = ETargetSelectionStrategy.Area;
        }

        [RelayCommand]
        public void SetCharacter()
        {
            if (Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

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

        #region Target size

        public bool ShowSizesCollection => Sizes?.Any() == true;

        private ObservableCollection<TargetSizeVM> _sizes = [];
        public ObservableCollection<TargetSizeVM> Sizes
        {
            get => _sizes;
            set => SetProperty(ref _sizes, value);
        }

        [RelayCommand]
        public async Task AddSize()
        {
            if (Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            List<object> allSizes = Enum.GetValues<ETargetSize>()
                .Select(x => new TargetSizeVM
                {
                    Size = x,
                    Text = Localization[x.ToString("G")].ToString() ?? string.Empty
                })
                .Where(x => !Sizes.Any(y => y.Size == x.Size))
                .Select(x => x as object)
                .ToList();

            PickerPopupResult? result = await ShowPopupAsync<PickerPopup, PickerPopupResult, PickerPopupParameters>(
                new()
                {
                    Title = Resources.Localizations.Localization.TargetsSizes,
                    Items = allSizes,
                    SelectedItems = [allSizes.First()],
                    SelectMultiple = true
                }
            );

            if (result != null)
            {
                List<TargetSizeVM> concreteResult = result.SelectedItems.Select(x => (TargetSizeVM)x).ToList();

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

            CostMonitor?.UpdateCost();

            OnPropertyChanged(nameof(ShowSizesCollection));
        }

        [RelayCommand]
        public void DeleteSize(TargetSizeVM property)
        {
            if (Aspect == null)
            {
                throw new Exception("Не инициализирована модель" + nameof(Aspect));
            }

            Sizes.Remove(property);
            Aspect.Internal.TargetsSizes.RemoveSize(property.Size);

            CostMonitor?.UpdateCost();

            OnPropertyChanged(nameof(ShowSizesCollection));
        }

        #endregion

        #region Area shape

        [RelayCommand]
        public void SetShape(string shape)
        {
            if(Aspect == null)
            {
                throw new Exception("Аспект не инициализирован.");
            }

            if (Enum.TryParse(shape, out EAreaType parsedShape))
            {
                Shape = parsedShape;
                Aspect.AreaShape.AreaType = parsedShape;
            }
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
                OnPropertyChanged(nameof(IsVoxelArray));
            }
        }

        public bool IsBrick => Shape == EAreaType.Brick;
        public bool IsCone => Shape == EAreaType.Cone;
        public bool IsCylinder => Shape == EAreaType.Cylinder;
        public bool IsSphere => Shape == EAreaType.Sphere;
        public bool IsVoxelArray => Shape == EAreaType.VoxelArray;

        #endregion
    }

    public class TargetSizeVM
    {
        public ETargetSize Size { get; set; }
        public string Text { get; set; } = string.Empty;

        public override string ToString() => Text;
    }
}
