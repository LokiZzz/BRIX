using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class HealEffectPage : ContentPage
{
	public HealEffectPage(DiceImpactEffectPageVMBase<HealEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}