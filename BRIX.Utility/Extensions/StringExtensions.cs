using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Utility.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? string.Empty
                : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
        }
    }
}
