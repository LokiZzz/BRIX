using BRIX.Library.Effects;
using BRIX.Mobile.Models.Abilities.Effects;

namespace BRIX.Mobile.ViewModel.Abilities.Effects
{
    public partial class SinglePropEffectPageVMBase<T> : EffectPageVMBase<EffectGenericModelBase<T>> 
        where T : SinglePropEffectBase, new()
    {
        protected override void HandleInitial(IDictionary<string, object> query)
        {
            base.HandleInitial(query);

            if(Effect == null)
            {
                throw new Exception("Эффект не инициализирован.");
            }

            OnPropertyChanged(nameof(Impact));
        }

        public int Impact
        {
            get => Effect?.Internal.Impact ?? 0;
            set 
            {
                if (Effect != null)
                {
                    SetProperty(Effect.Internal.Impact, value, Effect.Internal, (model, prop) =>
                    {
                        model.Impact = prop;
                        CostMonitor?.UpdateCost();
                    });
                }
            }
        }
    }
}
