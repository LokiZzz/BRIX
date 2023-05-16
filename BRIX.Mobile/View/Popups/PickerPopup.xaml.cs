using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class PickerPopup : Popup
{
    public PickerPopup(PickerPopupVM context)
    {
        InitializeComponent();
        context.View = this;
        BindingContext = context;
    }
}