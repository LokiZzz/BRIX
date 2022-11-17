using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class CharacterListPage : ContentPage
{
	public CharacterListPage(CharacterListPageVM context)
    {
        InitializeComponent();
        BindingContext = context;
    }
}