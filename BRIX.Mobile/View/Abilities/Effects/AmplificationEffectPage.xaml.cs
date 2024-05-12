using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class AmplificationEffectPage : ContentPage
{
	public AmplificationEffectPage(SinglePropEffectPageVMBase<AmplificationEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}