using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CharacterAbilitiesPage : ContentPage
{
	public CharacterAbilitiesPage(CharacterAbilitiesPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}