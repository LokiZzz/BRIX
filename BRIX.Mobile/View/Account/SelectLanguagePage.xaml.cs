using BRIX.Mobile.ViewModel.Account;

namespace BRIX.Mobile.View.Account;

public partial class SelectLanguagePage : ContentPage
{
	public SelectLanguagePage(SelectLanguagePageVM context)
	{
		InitializeComponent();
        BindingContext = context;
    }
}