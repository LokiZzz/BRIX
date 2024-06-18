using BRIX.Mobile.ViewModel.NPCs;

namespace BRIX.Mobile.View.NPCs;

public partial class EncounterCalculatorPage : ContentPage
{
	public EncounterCalculatorPage(EncounterCalculatorPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}