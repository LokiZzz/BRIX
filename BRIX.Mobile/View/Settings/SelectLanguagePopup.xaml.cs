using BRIX.Mobile.ViewModel.Settings;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Settings;

public partial class SelectLanguagePopup : Popup
{
	public SelectLanguagePopup(SelectLanguagePopupVM context)
	{
        InitializeComponent();
        context.View = this;
        BindingContext = context;
    }
}