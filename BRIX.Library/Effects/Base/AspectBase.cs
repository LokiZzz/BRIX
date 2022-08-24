using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.Base
{
    public abstract class AspectBase
    {
        public virtual bool IsEnabled { get; set; }

        public abstract double GetCoefficient();
    }
}
