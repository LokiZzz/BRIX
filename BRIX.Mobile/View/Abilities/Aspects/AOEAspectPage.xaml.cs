using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class AOEAspectPage : ContentPage
{
	public AOEAspectPage(AOEAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}