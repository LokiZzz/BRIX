using BRIX.Mobile.ViewModel.Character;

namespace BRIX.Mobile.View.Character;

public partial class CurrentCharacterPage : ContentPage
{
	public CurrentCharacterPage(CurrentCharacterPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}