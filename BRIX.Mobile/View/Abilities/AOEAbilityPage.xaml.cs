using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.View.Abilities;

public partial class AOEAbilityPage : ContentPage
{
	public AOEAbilityPage(AOEAbilityPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}