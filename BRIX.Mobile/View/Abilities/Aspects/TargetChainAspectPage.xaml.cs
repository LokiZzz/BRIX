using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class TargetChainAspectPage : ContentPage
{
	public TargetChainAspectPage(TargetChainAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}