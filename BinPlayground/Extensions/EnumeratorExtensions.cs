using System;
using System.Collections.Generic;

namespace BinPlayground.Extensions
{
    public static class EnumeratorExtensions
    {
        public static T[] ToArray<T>(this IEnumerator<T> enumerator)
        {
            if (enumerator == null)
            {
                throw new ArgumentNullException(nameof(enumerator));
            }

            var list = new List<T>();

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return list.ToArray();
        }
    }
}
