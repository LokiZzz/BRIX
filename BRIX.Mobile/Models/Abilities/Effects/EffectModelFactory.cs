using BRIX.Library.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Mobile.Models.Abilities.Effects
{
    public static class EffectModelFactory
    {
        public static EffectModel GetModel(EffectBase effect)
        {
            switch(effect)
            {
                case DamageEffect damage:
                    return new DamageEffectModel(damage);
            }

            return null;
        }
    }
}
