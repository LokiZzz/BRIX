using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class NumericEditorPopup : ParametrizedPopup<NumericEditorParameters>
{
	public NumericEditorPopup(NumericEditorPopupVM context)
	{
		InitializeComponent();
		context.View = this;
		context.PassInParameters = Parameters;
		BindingContext = context;
	}
}