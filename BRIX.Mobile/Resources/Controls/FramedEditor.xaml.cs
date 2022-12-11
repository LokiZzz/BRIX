namespace BRIX.Mobile.Resources.Controls;

public partial class FramedEditor : Grid
{
	public FramedEditor()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty TextProperty = BindableProperty.Create(
		propertyName: nameof(Text),
		returnType: typeof(string),
		declaringType: typeof(FramedEditor),
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
        declaringType: typeof(FramedEditor),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay
    );

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        propertyName: nameof(IsReadOnly),
        returnType: typeof(bool),
        declaringType: typeof(FramedEditor),
        defaultValue: false,
        defaultBindingMode: BindingMode.OneWay
    );

    public new bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        propertyName: nameof(MaxLength),
        returnType: typeof(int),
        declaringType: typeof(FramedEditor),
        defaultValue: 0,
        defaultBindingMode: BindingMode.OneWay
    );

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public string LimitText => $"{Text?.Length ?? 0}/{MaxLength}";

    private void Editor_Focused(object sender, FocusEventArgs e)
    {
        Up();
    }

    private void Editor_Unfocused(object sender, FocusEventArgs e)
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
        editor.Focus();
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            if (!editor.IsFocused)
            {
                Down();
            }
        }
        else
        {
            Up();
        }

        OnPropertyChanged(nameof(LimitText));
    }

    private void Down()
    {
        lblPlaceholder.FontSize = 15;
        lblPlaceholder.TranslateTo(0, 0, 100, Easing.BounceIn);
        lblPlaceholder.VerticalOptions = LayoutOptions.Center;
    }

    private void Up()
    {
        lblPlaceholder.FontSize = 13;
        lblPlaceholder.TranslateTo(0, -8, 100, Easing.BounceIn);
        lblPlaceholder.VerticalOptions = LayoutOptions.Start;
    }

    private void this_Loaded(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            Down();
        }
        else
        {
            Up();
        }

        OnPropertyChanged(nameof(LimitText));
    }
}