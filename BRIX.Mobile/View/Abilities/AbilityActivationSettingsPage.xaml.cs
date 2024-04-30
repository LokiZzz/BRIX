using BRIX.Mobile.ViewModel.Abilities;
using BRIX.Mobile.ViewModel.Abilities.Aspects;

namespace BRIX.Mobile.View.Abilities;

public partial class AbilityActivationSettingsPage : ContentPage
{
	public AbilityActivationSettingsPage(AbilityActivationSettingsPageVM context)
	{
		InitializeComponent();
		BindingContext = context;
	}
}