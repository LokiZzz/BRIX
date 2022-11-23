using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CharacterDetailsPage : ContentPage
{
	public CharacterDetailsPage(CharacterDetailsPageVM context)
	{
		InitializeComponent();
		BindingContext= context;
	}
}