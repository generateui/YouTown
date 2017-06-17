using System;
using System.Collections.Generic;

namespace YouTown
{
    /// <summary>
    /// Adds given dictionary to extended dictionary
    /// </summary>
    /// ***IMPURE***!
    /// Add for convenience in the <see cref="Converter"/> class. Do not use. If you do use it,
    /// make sure to not use this. Please do not use.
    /// <see cref="https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/"/>
    public static class ImpureExtensions
    {

        public static void AddDictionary<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, 
            IDictionary<TKey, TValue> toAdd)
        {
            foreach (KeyValuePair<TKey, TValue> pair in toAdd)
            {
                dictionary.Add(pair);
            }
        }

        public static void AddEnumerable<T>(this ISet<T> set, IEnumerable<T> sequence)
        {
            foreach (var item in sequence)
            {
                set.Add(item);
            }
        }

        public static void AddEnumerable<T>(this IList<T> list, IEnumerable<T> sequence)
        {
            foreach (var item in sequence)
            {
                list.Add(item);
            }
        }

    }
}
