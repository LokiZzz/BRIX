using BRIX.Mobile.ViewModel.Settings;

namespace BRIX.Mobile.View.Settings;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}