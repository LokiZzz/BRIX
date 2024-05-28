using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class MoveCharacterEffectPage : ContentPage
{
	public MoveCharacterEffectPage(MoveCharacterEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}