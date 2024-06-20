using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class DifficultTerrainEffectPage : ContentPage
{
	public DifficultTerrainEffectPage(SinglePropEffectPageVMBase<DifficultTerrainEffect> context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}