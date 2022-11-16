using BRIX.Mobile.ViewModel.Character;

namespace BRIX.Mobile.View.Character;

public partial class AddOrEditCharacterPage : ContentPage
{
	public AddOrEditCharacterPage(AddOrEditCharacterPageVM context)
    {
        InitializeComponent();
        BindingContext = context;
    }
}