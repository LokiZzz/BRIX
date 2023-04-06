using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel.Base;
using BRIX.Mobile.Models.Abilities.Effects;
using CommunityToolkit.Mvvm.ComponentModel;
using BRIX.Mobile.Models.Abilities.Aspects;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Aspects
{
    public partial class ActionPointAspectPageVM : AspectPageVMBase<ActionPointsAspectModel>
    {
        private int _actionPoints = 1;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (SetProperty(ref _actionPoints, value))
                {
                    Aspect.Internal.ActionPoints = value;
                    CostMonitor.UpdateCost();
                }
            }
        }

        [RelayCommand]
        private void SetPoints(string points)
        {
            int intPoints = int.Parse(points);
            ActionPoints = intPoints;
        }

        public override void Initialize(IDictionary<string, object> query)
        {
            ActionPoints = Aspect.Internal.ActionPoints;
        }
    }
}
