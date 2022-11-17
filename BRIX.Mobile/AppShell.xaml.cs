using BRIX.Mobile.Services;
using BRIX.Mobile.Services.Navigation;
using BRIX.Mobile.View.Characters;
using BRIX.Mobile.ViewModel.Base;

namespace BRIX.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Navigated += NavigatedHandler;
	}

	private async void NavigatedHandler(object sender, ShellNavigatedEventArgs e)
	{
		INavigationService navigation = ServicePool.GetService<INavigationService>();
		await navigation.FireOnNavigatedAsync();
    }
}