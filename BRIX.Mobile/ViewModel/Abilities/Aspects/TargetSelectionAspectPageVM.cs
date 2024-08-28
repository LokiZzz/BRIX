using BRIX.Library.Aspects.TargetSelection;
using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.Services;
using CommunityToolkit.Mvvm.Input;

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
}
