using BRIX.Mobile.ViewModel.ArmoryShop;
using BRIX.Mobile.ViewModel.Settings;

namespace BRIX.Mobile.View.ArmoryShop;

public partial class ArmoryShopPage : ContentPage
{
	public ArmoryShopPage(ArmoryShopPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}