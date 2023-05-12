using BRIX.Library.Aspects;
using BRIX.Library.Aspects.TargetSelection;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class TargetSelectionAspectPageVM : AspectPageVMBase<TargetSelectionAspectModel>
    {
        private ObservableCollection<TargetSelectionRestrictionPropertyVM> _restrictions;
        public ObservableCollection<TargetSelectionRestrictionPropertyVM> Restrictions
        {
            get => _restrictions;
            set => SetProperty(ref _restrictions, value);
        }

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

            Restrictions = new (Aspect.Internal.TargetSelectionRestrictions.Conditions.Select(ToRestrictionsVM));
        }

        private object ToRestrictionsVM(object arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private bool _isNTAD = true;
        public bool IsNTAD
        {
            get => _isNTAD;
            set
            {
                SetProperty(ref _isNTAD, value);
                OnPropertyChanged(nameof(ShowChainSettings));
            }
        }


        private bool _isAREA = true;
        public bool IsAREA
        {
            get => _isAREA;
            set
            {
                SetProperty(ref _isAREA, value);
                OnPropertyChanged(nameof(ShowChainSettings));
            }
        }

        private bool _isCharacter = true;
        public bool IsCharacter
        {
            get => _isCharacter;
            set
            {
                SetProperty(ref _isCharacter, value);
                OnPropertyChanged(nameof(ShowChainSettings));
            }
        }

        public bool ShowChainSettings => IsNTAD || IsAREA;

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

    public class TargetSelectionRestrictionPropertyVM
    {
        public string Restriction { get; set; }
    }
}
