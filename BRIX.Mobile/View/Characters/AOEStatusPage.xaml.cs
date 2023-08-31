using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class AOEStatusPage : ContentPage
{
	public AOEStatusPage(AOEStatusPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}