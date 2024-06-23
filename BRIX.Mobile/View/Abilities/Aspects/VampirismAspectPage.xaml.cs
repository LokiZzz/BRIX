using BRIX.Mobile.Models.Abilities.Aspects;
using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities.Aspects;

public partial class VampirismAspectPage : ContentPage
{
	public VampirismAspectPage(VampirismAspectPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}