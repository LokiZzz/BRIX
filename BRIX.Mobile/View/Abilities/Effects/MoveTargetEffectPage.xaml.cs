using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class MoveTargetEffectPage : ContentPage
{
	public MoveTargetEffectPage(MoveTargetEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}