using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.ViewModel;

namespace BRIX.Mobile;

public partial class AppShell : Shell
{
	public AppShell(AppShellVM context)
	{
		InitializeComponent();
		Navigated += NavigatedHandler;
		BindingContext = context;
	}

	private async void NavigatedHandler(object sender, ShellNavigatedEventArgs e)
	{
		INavigationService navigation = ServicePool.GetService<INavigationService>();
		await navigation.FireOnNavigatedAsync();
    }
}