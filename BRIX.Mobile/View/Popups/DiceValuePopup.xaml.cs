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
        context.OnInvalidFormulaEntered += PlayInavalidFormulaAnimation;
        BindingContext = context;
    }

    private async void PlayInavalidFormulaAnimation(object sender, EventArgs e)
    {
        uint length = 60;

        Color before = formulaEntry.EntryColor.Copy();
        Color after = Colors.Red;

        Application.Current.Resources.TryGetValue("BRIXRed", out object colorResource);
        if (colorResource != null && colorResource is Color afterColor)
        {
            after = afterColor;
        }

        Animation animation = new (x => formulaEntry.EntryColor = GetColorTransitionState(before, after, x));
        Animation revertAnimation = new(x => formulaEntry.EntryColor = GetColorTransitionState(after, before, x));

        animation.Commit(formulaEntry, "ToColorAnimation", 16, 250, Easing.Linear, (x, y) => 
            revertAnimation.Commit(formulaEntry, "RevertColorAnimation", 16, 500, Easing.Linear)
        );

        await formulaEntry.TranslateTo(5, 0, length);
        await formulaEntry.TranslateTo(-5, 0, length);
        await formulaEntry.TranslateTo(5, 0, length);
        await formulaEntry.TranslateTo(-5, 0, length);
        await formulaEntry.TranslateTo(5, 0, length);
        await formulaEntry.TranslateTo(0, 0, length);
    }

    private Color GetColorTransitionState(Color from, Color to, double transitionCoef)
    {
        return new Color(
            Transition(from.Red, to.Red, transitionCoef),
            Transition(from.Green, to.Green, transitionCoef),
            Transition(from.Blue, to.Blue, transitionCoef)
        );

        float Transition(double from, double to, double coef)
        {
            return (float)(from - (from - to) * coef);
        }
    }
}