using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class RoundDurationAspectPage : ContentPage
{
	public RoundDurationAspectPage(RoundDurationAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}