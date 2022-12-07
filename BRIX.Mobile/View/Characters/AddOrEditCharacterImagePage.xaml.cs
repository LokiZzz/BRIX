using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class AddOrEditCharacterImagePage : ContentPage
{
	public AddOrEditCharacterImagePage(AddOrEditCharacterImagePageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}