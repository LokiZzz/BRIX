using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class DecelerationEffectPage : ContentPage
{
	public DecelerationEffectPage(DiceImpactEffectPageVMBase<DecelerationEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}