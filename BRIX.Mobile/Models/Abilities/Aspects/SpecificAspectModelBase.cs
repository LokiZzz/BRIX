using BRIX.Library.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Aspects
{
    public abstract class SpecificAspectModelBase<T>(T model) : AspectModelBase(model) where T : AspectBase, new()
    {
        public T Internal => (T)base.Internal;
    }
}
