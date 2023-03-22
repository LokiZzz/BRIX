using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class ActionPointAspectPage : ContentPage
{
	public ActionPointAspectPage(ActionPointAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}