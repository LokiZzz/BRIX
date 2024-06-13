using BRIX.Mobile.ViewModel.NPCs;

namespace BRIX.Mobile.View.NPCs;

public partial class AOENPCsPage : ContentPage
{
	public AOENPCsPage(AOENPCsPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}