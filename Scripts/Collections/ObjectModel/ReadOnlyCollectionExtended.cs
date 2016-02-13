using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DRG.Collections.ObjectModel
{
    static public class MethodExtensionForReadOnlyCollection
    {
        /// <summary>
        /// </summary>
        static public List<T> FindAll<T>(this ReadOnlyCollection<T> collection, Predicate<T> predicate)
        {
            List<T> result = new List<T>();

            for (int i = 0; i < collection.Count; i++)
            {
                T element = collection[i];

                if (predicate(element) == true)
                {
                    result.Add(element);
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        static public T Find<T>(this ReadOnlyCollection<T> collection, Predicate<T> predicate)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                T element = collection[i];

                if (predicate(element) == true)
                {
                    return element;
                }
            }

            return default(T);
        }
    }
}
