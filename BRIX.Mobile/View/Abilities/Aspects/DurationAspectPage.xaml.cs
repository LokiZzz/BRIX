using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class DurationAspectPage : ContentPage
{
	public DurationAspectPage(DurationAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}