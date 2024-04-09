using BRIX.Library.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public abstract class SpecificAspectModelBase<T> : AspectModelBase where T : AspectBase, new()
    {
        public SpecificAspectModelBase(T model) : base(model) { }

        public T Internal => (T)InternalModel;
    }
}
