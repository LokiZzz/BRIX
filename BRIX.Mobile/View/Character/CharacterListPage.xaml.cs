using BRIX.Mobile.ViewModel.Character;

namespace BRIX.Mobile.View.Character;

public partial class CharacterListPage : ContentPage
{
	public CharacterListPage(CharacterListPageVM context)
    {
        InitializeComponent();
        BindingContext = context;
    }
}