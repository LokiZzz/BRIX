using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CurrentCharacterPage : ContentPage
{
	public CurrentCharacterPage(CurrentCharacterPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}