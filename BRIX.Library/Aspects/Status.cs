using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Aspects
{
    public class Status
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }
    }
}
