using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class AddHealthPage : ContentPage
{
	public AddHealthPage(AddHealthPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}