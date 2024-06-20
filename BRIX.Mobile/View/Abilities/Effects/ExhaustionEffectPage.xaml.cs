using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class ExhaustionEffectPage : ContentPage
{
	public ExhaustionEffectPage(DiceImpactEffectPageVMBase<ExhaustionEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}