using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Ability
{
    public class AbilityLogicException : Exception 
    {
        public AbilityLogicException(string message) : base(message) { }
    }
}
