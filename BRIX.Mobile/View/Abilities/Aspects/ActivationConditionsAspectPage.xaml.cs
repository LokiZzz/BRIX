using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class ActivationConditionsAspectPage : ContentPage
{
	public ActivationConditionsAspectPage(ActivationConditionsAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}