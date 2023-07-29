using BRIX.Mobile.ViewModel.Details;

namespace BRIX.Mobile.View.Details;

public partial class AddOrEditProjectPage : ContentPage
{
	public AddOrEditProjectPage(AddOrEditProjectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}