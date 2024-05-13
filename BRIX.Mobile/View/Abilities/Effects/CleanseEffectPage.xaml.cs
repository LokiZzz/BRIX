using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class CleanseEffectPage : ContentPage
{
	public CleanseEffectPage(EffectPageVMBase<CleanseEffectModel> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}