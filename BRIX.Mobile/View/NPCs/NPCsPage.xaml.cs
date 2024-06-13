using BRIX.Mobile.ViewModel.NPCs;

namespace BRIX.Mobile.View.NPCs;

public partial class NPCsPage : ContentPage
{
	public NPCsPage(NPCsPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}