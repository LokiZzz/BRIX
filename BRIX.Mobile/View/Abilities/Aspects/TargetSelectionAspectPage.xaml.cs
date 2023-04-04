using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class TargetSelectionAspectPage : ContentPage
{
	public TargetSelectionAspectPage(TargetSelectionAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}