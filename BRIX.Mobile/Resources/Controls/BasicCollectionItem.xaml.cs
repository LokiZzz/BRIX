using System.Windows.Input;

namespace BRIX.Mobile.Resources.Controls;

public partial class BasicCollectionItem : ContentView
{
	public BasicCollectionItem()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(BasicCollectionItem)
    );

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty SecondaryTextProperty = BindableProperty.Create(
        nameof(SecondaryText),
        typeof(string),
        typeof(BasicCollectionItem)
    );

    public string SecondaryText
    {
        get { return (string)GetValue(SecondaryTextProperty); }
        set { SetValue(SecondaryTextProperty, value); }
    }

    public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(
        nameof(DeleteCommand),
        typeof(ICommand),
        typeof(BasicCollectionItem)
    );

    public ICommand DeleteCommand
    {
        get { return (ICommand)GetValue(DeleteCommandProperty); }
        set { SetValue(DeleteCommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(BasicCollectionItem)
    );

    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }
}