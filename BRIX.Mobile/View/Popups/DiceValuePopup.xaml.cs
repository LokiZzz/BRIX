using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Maui.Views;

namespace BRIX.Mobile.View.Popups;

public partial class DiceValuePopup : Popup
{
	public DiceValuePopup(DiceValuePopupVM context)
	{
        InitializeComponent();
        context.View = this;
        context.OnInvalidFormulaEntered += PlayAnimation;
        BindingContext = context;
    }

    private void PlayAnimation(object? sender, EventArgs e)
    {
        AnimationHelper.PlayInvalidEntryAnimation(formulaEntry);
    }
}