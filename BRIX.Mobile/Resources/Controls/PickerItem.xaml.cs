namespace BRIX.Mobile.Resources.Controls;

public partial class PickerItem : Grid
{
	public PickerItem()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        propertyName: nameof(Text),
        returnType: typeof(string),
        declaringType: typeof(PickerItem),
        defaultBindingMode: BindingMode.OneWay
    );

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}