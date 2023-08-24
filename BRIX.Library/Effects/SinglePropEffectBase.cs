using BRIX.Library.DiceValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects
{
    public abstract class SinglePropEffectBase : EffectBase
    {
        public DicePool Impact { get; set; } = new DicePool();
    }
}
