using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class AddOrEditCharacterPage : ContentPage
{
	public AddOrEditCharacterPage(AddOrEditCharacterPageVM context)
    {
        InitializeComponent();
        BindingContext = context;
    }
}