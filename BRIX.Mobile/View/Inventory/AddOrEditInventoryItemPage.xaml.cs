using BRIX.Mobile.ViewModel.Inventory;

namespace BRIX.Mobile.View.Inventory;

public partial class AddOrEditInventoryItemPage : ContentPage
{
	public AddOrEditInventoryItemPage(AddOrEditInventoryItemPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}