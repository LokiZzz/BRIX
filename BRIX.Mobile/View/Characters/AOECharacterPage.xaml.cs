using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class AOECharacterPage : ContentPage
{
	public AOECharacterPage(AOECharacterPageVM context)
    {
        InitializeComponent();
        BindingContext = context;
    }
}