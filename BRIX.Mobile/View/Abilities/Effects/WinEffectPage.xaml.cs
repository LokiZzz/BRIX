using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class WinEffectPage : ContentPage
{
	public WinEffectPage(EffectPageVMBase<EffectGenericModelBase<WinTheGameEffect>> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}