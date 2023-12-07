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
        [RelayCommand]
        private void SetPoints(string points)
        {
            if (Aspect != null)
            {
                int intPoints = int.Parse(points);
                Aspect.ActionPoints = intPoints;
            }
        }
    }
}
