using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class AlertPopup : Popup
{
	public AlertPopup(AlertPopupVM context)
	{
        InitializeComponent();
        context.View = this;
        BindingContext = context;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}