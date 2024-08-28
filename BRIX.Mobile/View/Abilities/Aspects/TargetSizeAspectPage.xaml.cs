using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class TargetSizeAspectPage : ContentPage
{
	public TargetSizeAspectPage(TargetSizeAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}