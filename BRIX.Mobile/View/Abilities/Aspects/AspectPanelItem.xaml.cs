namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class AspectPanelItem : ContentView
{
	public AspectPanelItem()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty ItemBackgroundColorProperty =
        BindableProperty.Create(nameof(ItemBackgroundColor), typeof(Color), typeof(AspectPanelItem));

    public Color ItemBackgroundColor
    {
        get { return (Color)GetValue(ItemBackgroundColorProperty); }
        set { SetValue(ItemBackgroundColorProperty, value); }
    }
}