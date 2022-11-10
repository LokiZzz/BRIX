using BRIX.Mobile.ViewModel.Account;

namespace BRIX.Mobile.View.Account;

public partial class SignInPage : ContentPage
{
	public SignInPage(SignInPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}