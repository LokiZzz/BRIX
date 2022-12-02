using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class QuestionPopup : Popup
{
	public QuestionPopup(QuestionPopupVM context)
	{
        InitializeComponent();
        context.View = this;
        BindingContext = context;
    }
}