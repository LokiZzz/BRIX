using BRIX.Library.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public class EffectGenericModelBase<T> : EffectModelBase where T : EffectBase, new()
    {
        public EffectGenericModelBase() : this(new T()) { }

        public EffectGenericModelBase(T model)
        {
            InternalModel = model;
            model.ForceAspectInitialize();
            UpdateAspects();
        }

        public T Internal => GetSpecificEffect<T>();
    }
}
