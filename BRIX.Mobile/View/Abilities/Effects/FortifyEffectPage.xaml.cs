using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class FortifyEffectPage : ContentPage
{
	public FortifyEffectPage(DiceImpactEffectPageVMBase<FortifyEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}