using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class SelfDamageEffectPage : ContentPage
{
	public SelfDamageEffectPage(DiceImpactEffectPageVMBase<SelfDamageEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}