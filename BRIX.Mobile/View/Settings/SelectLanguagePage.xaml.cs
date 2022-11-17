using BRIX.Mobile.ViewModel.Settings;

namespace BRIX.Mobile.View.Settings;

public partial class SelectLanguagePage : ContentPage
{
	public SelectLanguagePage(SelectLanguagePageVM context)
	{
		InitializeComponent();
        BindingContext = context;
    }
}