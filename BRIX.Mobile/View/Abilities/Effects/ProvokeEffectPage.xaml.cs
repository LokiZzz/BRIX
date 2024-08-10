using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class ProvokeEffectPage : ContentPage
{
	public ProvokeEffectPage(EffectPageVMBase<EffectGenericModelBase<ProvokeEffect>> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}