using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Characters
{
    public class NotEnoughEXPForChangesException : Exception
    {
        public NotEnoughEXPForChangesException(string message) : base(message) { }
    }
}
