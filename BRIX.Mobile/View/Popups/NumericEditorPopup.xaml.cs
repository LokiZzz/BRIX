using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class NumericEditorPopup : Popup
{
	public NumericEditorPopup(NumericEditorPopupVM context)
	{
		InitializeComponent();
		context.View = this;
		BindingContext = context;
	}
}