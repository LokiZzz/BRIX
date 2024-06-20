using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class DefenseEffectPage : ContentPage
{
	public DefenseEffectPage(DiceImpactEffectPageVMBase<DefenseEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}