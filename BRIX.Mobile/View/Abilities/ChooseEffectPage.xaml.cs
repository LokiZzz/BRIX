using BRIX.Mobile.ViewModel.Abilities;

namespace BRIX.Mobile.View.Abilities;

public partial class ChooseEffectPage : ContentPage
{
	public ChooseEffectPage(ChooseEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}