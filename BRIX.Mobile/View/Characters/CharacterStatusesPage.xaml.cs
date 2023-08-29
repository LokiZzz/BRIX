using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CharacterStatusesPage : ContentPage
{
	public CharacterStatusesPage(CharacterStatusesPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}