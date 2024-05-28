using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class MoveAreaEffectPage : ContentPage
{
	public MoveAreaEffectPage(EffectPageVMBase<MoveAreaEffectModel> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}