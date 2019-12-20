using System;
using System.Collections.Generic;

namespace Digipolis.Web.Api.Tools
{
    public static class DictionaryExtensions
    {
        public static void AddRangeIfNotExist<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
            }
        }
    }
}