using BRIX.Library.Aspects.TargetSelection;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class AOEAspectPageVM() : AspectPageVMBase<AOEAspectModel>
    {
        public override void Initialize()
        {
            if(Aspect == null)
            {
                throw new ArgumentNullException(nameof(Aspect));
            }

            SetShape(Aspect.AreaShape.AreaType.ToString("G"));
            Aspect.AreaShape.CostMonitor = Aspect.CostMonitor;
        }

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
                OnPropertyChanged(nameof(IsArbitrary));
            }
        }

        public bool IsBrick => Shape == EAreaType.Brick;
        public bool IsCone => Shape == EAreaType.Cone;
        public bool IsCylinder => Shape == EAreaType.Cylinder;
        public bool IsSphere => Shape == EAreaType.Sphere;
        public bool IsArbitrary => Shape == EAreaType.Arbitrary;
    }
}
