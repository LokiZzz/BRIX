using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class CooldownAspectPage : ContentPage
{
	public CooldownAspectPage(CooldownAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}