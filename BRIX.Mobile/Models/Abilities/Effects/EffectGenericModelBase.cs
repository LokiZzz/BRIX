using BRIX.Library.Effects;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class EffectGenericModelBase<T> : EffectModelBase where T : EffectBase, new()
    {
        public EffectGenericModelBase() : this(new T()) { }

        public EffectGenericModelBase(T model)
        {
            InternalModel = model;
            model.ForceAspectInitialize();
            InitializeAspects();
        }

        public T Internal => GetSpecificEffect<T>();
    }
}
