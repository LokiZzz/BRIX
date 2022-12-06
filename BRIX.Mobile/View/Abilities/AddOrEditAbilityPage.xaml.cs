using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.View.Abilities;

public partial class AddOrEditAbilityPage : ContentPage
{
	public AddOrEditAbilityPage(AddOrEditAbilityPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}