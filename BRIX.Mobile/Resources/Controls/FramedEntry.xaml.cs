namespace BRIX.Mobile.Resources.Controls;

public partial class FramedEntry : Grid
{
	public FramedEntry()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		propertyName: nameof(Text),
		returnType: typeof(string),
		declaringType: typeof(FramedEntry),
		defaultValue: null,
		defaultBindingMode: BindingMode.TwoWay
	);

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(FramedEntry),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay
    );

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        propertyName: nameof(IsPassword),
        returnType: typeof(bool),
        declaringType: typeof(FramedEntry),
        defaultValue: false,
        defaultBindingMode: BindingMode.OneWay
    );

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        propertyName: nameof(IsReadOnly),
        returnType: typeof(bool),
        declaringType: typeof(FramedEntry),
        defaultValue: false,
        defaultBindingMode: BindingMode.OneWay
    );

    public new bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        Up();
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            Down();
        }
        else
        {
            Up();
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        entry.Focus();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            if (!entry.IsFocused)
            {
                Down();
            }
        }
        else
        {
            Up();
        }
    }

    private void Down()
    {
        lblPlaceholder.FontSize = 15;
        lblPlaceholder.TranslateTo(0, 0, 100, Easing.BounceIn);
    }

    private void Up()
    {
        lblPlaceholder.FontSize = 13;
        lblPlaceholder.TranslateTo(0, -25, 100, Easing.BounceIn);
    }
}