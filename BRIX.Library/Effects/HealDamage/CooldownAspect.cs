using BRIX.Library.Effects.Base;
using BRIX.Library.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Effects.HealDamage
{
    public class CooldownAspect : AspectBase
    {
        public ECooldownOption Cooldown { get; set; }

        public override double GetCoefficient() => ((int)Cooldown).ToNegativeCoeficient();
    }

    public enum ECooldownOption
    {
        None = 0,
        Minute = 10,
        Hour = 15,
        Day = 20,
        Week = 25,
        Month = 30,
        Year = 35,
        TenYears = 40,
        HundredYears = 45,
        CannotReset = 70
    }
}
