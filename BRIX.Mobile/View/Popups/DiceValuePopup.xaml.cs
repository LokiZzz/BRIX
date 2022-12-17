using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class DiceValuePopup : Popup
{
	public DiceValuePopup(DiceValuePopupVM context)
	{
        InitializeComponent();
        context.View = this;
        BindingContext = context;
    }
}