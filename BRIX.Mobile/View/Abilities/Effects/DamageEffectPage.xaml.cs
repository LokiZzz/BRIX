using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class DamageEffectPage : ContentPage
{
	public DamageEffectPage(DamageEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}