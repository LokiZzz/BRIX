using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class ReductionEffectPage : ContentPage
{
	public ReductionEffectPage(DiceImpactEffectPageVMBase<ReductionEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}