using BRIX.Mobile.ViewModel.Popups;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class EntryPopup : Popup
{
	public EntryPopup(EntryPopupVM context)
    {
        InitializeComponent();
        context.View = this;
        context.OnEmptyValueEntered += PlayAnimation;
        BindingContext = context;
    }

    private void PlayAnimation(object? sender, EventArgs e)
    {
        AnimationHelper.PlayInvalidEntryAnimation(entry);
    }
}