using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class FortifyEffectPage : ContentPage
{
	public FortifyEffectPage(SinglePropEffectPageVMBase<FortifyEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}