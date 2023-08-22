using BRIX.Library.DiceValue;
using BRIX.Mobile.Models.Abilities.Effects;
using BRIX.Mobile.View.Popups;
using BRIX.Mobile.ViewModel.Popups;
using BRIX.Utility.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class DamageEffectPageVM : EffectPageVMBase<DamageEffectModel>
    {
        public override void Initialize()
        {
            DicePoolEditor.DicePoolUpdated += OnImpactUpdated;
            DicePoolEditor.Dices = Effect.Internal.Impact.IsEmpty 
                ? new DicePool((1, 4))
                : Effect.Internal.Impact;
        }

        private void OnImpactUpdated(object sender, EventArgs e)
        {
            Effect.Internal.Impact = DicePoolEditor.Dices;
            CostMonitor.UpdateCost();
        }

        private DicePoolEditorVM _dicePoolEditor = new();
        public DicePoolEditorVM DicePoolEditor
        {
            get => _dicePoolEditor;
            set => SetProperty(ref _dicePoolEditor, value);
        }
    }
}
