using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class PeriodicDamageEffectPage : ContentPage
{
	public PeriodicDamageEffectPage(SinglePropEffectPageVMBase<PeriodicDamageEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}