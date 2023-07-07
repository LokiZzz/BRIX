using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.Extensions
{
    public static class CommonExtensions
    {
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, 
            T oldValue, T newValue, IEqualityComparer<T>? comparer = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            comparer ??= EqualityComparer<T>.Default;

            foreach (T? item in source)
            {
                yield return comparer.Equals(item, oldValue) ? newValue : item;
            }
        }
    }
}
