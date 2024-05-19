using BRIX.Library.DiceValue;
using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class SinglePropEffectPageVMBase<T> : EffectPageVMBase<EffectGenericModelBase<T>> 
        where T : DiceImpactEffectBase, new()
    {
        protected override void HandleInitial(IDictionary<string, object> query)
        {
            base.HandleInitial(query);

            if(Effect == null)
            {
                throw new Exception("Эффект не инициализирован.");
            }

            DicePoolEditor.DicePoolUpdated += OnImpactUpdated;
            DicePoolEditor.Dices = Effect.Internal.Impact.IsEmpty
                ? new DicePool((1, 4))
                : Effect.Internal.Impact;
        }

        private void OnImpactUpdated(object? sender, EventArgs e)
        {
            if (Effect == null)
            {
                throw new Exception("Эффект не инициализирован.");
            }

            Effect.Internal.Impact = DicePoolEditor.Dices;
            CostMonitor?.UpdateCost();
        }

        private DicePoolEditorVM _dicePoolEditor = new();
        public DicePoolEditorVM DicePoolEditor
        {
            get => _dicePoolEditor;
            set => SetProperty(ref _dicePoolEditor, value);
        }
    }
}
