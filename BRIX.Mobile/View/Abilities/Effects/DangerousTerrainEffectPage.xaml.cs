using BRIX.Library.Effects;
using BRIX.Mobile.ViewModel.Abilities.Effects;

namespace BRIX.Mobile.View.Abilities.Effects;

public partial class DangerousTerrainEffectPage : ContentPage
{
	public DangerousTerrainEffectPage(DangerousTerrainEffectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}