using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class DamageEffectPage : ContentPage
{
	public DamageEffectPage(SinglePropEffectPageVMBase<DamageEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}