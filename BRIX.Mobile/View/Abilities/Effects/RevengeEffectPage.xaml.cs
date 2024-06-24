using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class RevengeEffectPage : ContentPage
{
	public RevengeEffectPage(SinglePropEffectPageVMBase<RevengeEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}