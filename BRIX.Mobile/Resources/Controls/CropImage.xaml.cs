namespace BRIX.Mobile.Resources.Controls;

public partial class CropImage : Grid
{
	public CropImage()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        propertyName: nameof(Source),
        returnType: typeof(string),
        declaringType: typeof(FramedEntry),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay
    );

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}