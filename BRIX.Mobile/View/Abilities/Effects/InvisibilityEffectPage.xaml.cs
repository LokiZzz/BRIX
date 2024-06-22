using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class InvisibilityEffectPage : ContentPage
{
	public InvisibilityEffectPage(EffectPageVMBase<EffectGenericModelBase<InvisibiltyEffect>> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}