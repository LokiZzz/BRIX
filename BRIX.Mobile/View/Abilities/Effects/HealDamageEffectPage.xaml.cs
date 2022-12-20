using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class HealDamageEffectPage : ContentPage
{
	public HealDamageEffectPage(HealDamageEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}