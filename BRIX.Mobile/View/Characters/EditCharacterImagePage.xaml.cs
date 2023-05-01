using BRIX.Mobile.ViewModel.Characters;

namespace BRIX.Mobile.View.Characters;

public partial class EditCharacterImagePage : ContentPage
{
	public EditCharacterImagePage(EditCharacterImagePageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}