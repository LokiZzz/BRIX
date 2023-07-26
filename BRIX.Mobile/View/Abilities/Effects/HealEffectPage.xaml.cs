using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class HealEffectPage : ContentPage
{
	public HealEffectPage(HealEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}