using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class EditSpeedPage : ContentPage
{
	public EditSpeedPage(EditSpeedPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}