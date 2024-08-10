using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class MutenessEffectPage : ContentPage
{
	public MutenessEffectPage(EffectPageVMBase<MutenessEffectModel> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}