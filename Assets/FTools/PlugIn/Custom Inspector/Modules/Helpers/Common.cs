using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace CustomInspector.Helpers
{
    public static class Common
    {
        /// <summary>
        /// Gets the first item that matches the predicate
        /// </summary>
        /// <returns>If any item was found</returns>
        public static bool TryGetFirst<T>(this IEnumerable<T> list, Func<T, bool> predicate, out T match)
        {
            foreach (T item in list)
            {
                if(predicate(item))
                {
                    match = item;
                    return true;
                }
            }
            match = default;
            return false;
        }
        public static bool SequenceEqual(this IList list1, IList list2)
        {
            if (list1.Count != list2.Count)
                return false;

            try
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (!list1[i].Equals(list2[i]))
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
            return true;
        }
    }
}
