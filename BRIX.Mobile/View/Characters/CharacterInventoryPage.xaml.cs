using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CharacterInventoryPage : ContentPage
{
	public CharacterInventoryPage(CharacterInventoryPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}