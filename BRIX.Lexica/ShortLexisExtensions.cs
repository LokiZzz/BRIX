using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Lexica
{
    public static class ShortLexisExtensions
    {
        public static MarkupString ToAggregatedString<T>(
            this IEnumerable<T> collection, 
            Func<T, string> toStringDelegate)
        {
            if(collection.Count() == 0)
            {
                return (MarkupString)"[NONE]";
            }
            else
            {
                string result = '[' 
                    + collection.Select(toStringDelegate).Aggregate((x, y) => x + ',' + y) 
                    + "]";

                return (MarkupString)result;
            }
        }
    }
}
