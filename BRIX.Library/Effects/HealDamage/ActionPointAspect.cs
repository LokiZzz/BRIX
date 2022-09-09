using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRIX.Library.Effects.Base;

namespace BRIX.Library.Effects.HealDamage
{
    public class ActionPointAspect : AspectBase
    {
        public override bool IsEnabled { get; set; } = true;

        private int _minActionPoints = 1;
        private int _maxActionPoints = 50;

        private int _actionPoints = 1;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                if (value < _minActionPoints || _maxActionPoints < value)
                {
                    throw new ArgumentException("Способность может тратить от 1 до 50 очков действий.");
                }

                _actionPoints = value;
            }
        }

        public override double GetCoefficient()
        {
            int percents;

            switch (ActionPoints)
            {
                case <= 5:
                    percents = (ActionPoints - 1) * 5;
                    break;
                default:
                    percents = ActionPoints + 15;
                    break;
            }

            return 1 - (double)percents / 100;
        }
    }
}
