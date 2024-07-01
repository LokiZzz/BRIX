using BRIX.Mobile.ViewModel.Settings;

namespace BRIX.Mobile.View.Settings;

public partial class LogPage : ContentPage
{
	public LogPage(LogPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}