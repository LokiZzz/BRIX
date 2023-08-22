namespace BRIX.Mobile.View.Abilities;

public partial class DicePoolEditor : ContentView
{
	public DicePoolEditor()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty ShowFastAdjustmentProperty = BindableProperty.Create(
		nameof(ShowFastAdjustment), 
		typeof(bool), typeof(DicePoolEditor), 
		defaultValue: true
	);

    public bool ShowFastAdjustment
	{
		get { return (bool)GetValue(ShowFastAdjustmentProperty); }
		set { SetValue(ShowFastAdjustmentProperty, value); }
	}
}