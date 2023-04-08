using BRIX.Library.Aspects;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static BRIX.Library.Aspects.AreaSettings;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class TargetSelectionAspectPageVM : AspectPageVMBase<TargetSelectionAspectModel>
    {
        [RelayCommand]
        public void SetNTAD()
        {
            IsAREA = false;
            IsNTAD = true;
            Aspect.Strategy = ETargetSelectionStrategy.NTargetsAtDistanсeL;
        }

        [RelayCommand]
        public void SetAREA()
        {
            IsNTAD = false;
            IsAREA = true;
            Aspect.Strategy = ETargetSelectionStrategy.Area;
        }

        [RelayCommand]
        public void SetShape(string shape)
        {
            Enum.TryParse(shape, out EAreaType parsedShape);
            Shape = parsedShape;
            Aspect.AreaType = parsedShape;
        }

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
        }

        #region Strategy and Shape visibility flags

        [ObservableProperty]
        private bool _isNTAD = true;

        [ObservableProperty]
        private bool _isAREA = false;

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
}
