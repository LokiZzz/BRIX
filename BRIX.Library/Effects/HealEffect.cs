using BRIX.Library.DiceValue;
using BRIX.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects
{
    public class HealEffect : DamageEffect
    {
        public override int BaseExpCost() => (base.BaseExpCost() * 1.2).Round();
    }
}
