using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class RevengeEffectPage : ContentPage
{
	public RevengeEffectPage(EffectPageVMBase<EffectGenericModelBase<RevengeEffect>> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}