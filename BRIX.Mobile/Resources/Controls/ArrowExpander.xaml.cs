namespace BRIX.Mobile.Resources.Controls;

public partial class ArrowExpander : ContentView
{
	public ArrowExpander()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty ExpanderContentProperty = BindableProperty.Create(
        nameof(ExpanderContent),
        typeof(IView),
        typeof(ArrowExpander)
    );

    public IView ExpanderContent
    {
        get { return (IView)GetValue(ExpanderContentProperty); }
        set { SetValue(ExpanderContentProperty, value); }
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(ArrowExpander)
    );

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
}