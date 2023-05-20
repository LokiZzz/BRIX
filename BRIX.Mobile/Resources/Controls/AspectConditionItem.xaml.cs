using System.Windows.Input;

namespace BRIX.Mobile.Resources.Controls;

public partial class AspectConditionItem : ContentView
{
	public AspectConditionItem()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(AspectConditionItem)
    );

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(
        nameof(DeleteCommand),
        typeof(ICommand),
        typeof(AspectConditionItem)
    );

    public ICommand DeleteCommand
    {
        get { return (ICommand)GetValue(DeleteCommandProperty); }
        set { SetValue(DeleteCommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(AspectConditionItem)
    );

    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }
}