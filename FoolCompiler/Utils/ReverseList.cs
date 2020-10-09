using System;
using System.Collections.Generic;
using System.Text;

namespace FoolCompiler.Utils
{
    public static class ReverseList
    {
        public static IEnumerable<T> AsReverseEnumerator<T>(this IReadOnlyList<T> list)
        {
            for (int i = list.Count; --i >= 0;) yield return list[i];
        }
    }
}
