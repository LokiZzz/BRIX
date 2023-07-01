using BRIX.Mobile.Resources.Localizations;
using BRIX.Mobile.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace BRIX.Mobile.Resources.Controls;

public partial class HelpCard : ContentView
{
	public HelpCard()
	{
		InitializeComponent();
        IsVisible = false;
        WeakReferenceMessenger.Default.Register<ShowHelpCardsChanged>(this, UpdateVisibility);
    }

    private void UpdateVisibility(object recipient, ShowHelpCardsChanged message)
    {
        IsVisible = Preferences.Get(Help, true);
    }

    public static readonly BindableProperty HelpProperty = BindableProperty.Create(
        propertyName: nameof(Help),
        returnType: typeof(string),
        declaringType: typeof(HelpCard),
        defaultBindingMode: BindingMode.TwoWay,
        defaultValue: string.Empty,
        propertyChanged: HelpChanged
    );


    public string Help
    {
        get => (string)GetValue(HelpProperty);
        set => SetValue(HelpProperty, value);
    }
    
    public void SetText(string text)
    {
        lblHelpText.Text = text;
    }

    private static void HelpChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ILocalizationResourceManager localization = ServicePool.GetService<ILocalizationResourceManager>();
        string helpText = localization[(string)newValue].ToString();
        HelpCard helpCard = (HelpCard)bindable;
        helpCard.SetText(helpText);
        helpCard.IsVisible = Preferences.Get(helpCard.Help, true);
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        IsVisible = false;
        Preferences.Set(Help, false);
    }
}

public class ShowHelpCardsChanged { }