using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CharacterPage : ContentPage
{
	public CharacterPage(CharacterPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}