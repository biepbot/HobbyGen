namespace HobbyGen.Controllers.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extender class for additional list functionality
    /// </summary>
    public static class ListExtender
    {
        /// <summary>
        /// Check if a list is equals to this one, even when scrambled
        /// </summary>
        /// <typeparam name="T">The type of list</typeparam>
        /// <param name="t">The first list</param>
        /// <param name="o">The second list</param>
        /// <returns></returns>
        public static bool ScrambledEquals<T>(this IEnumerable<T> t, IEnumerable<T> o)
        {
            var deletedItems = t.Except(o).Any();
            var newItems = o.Except(t).Any();
            return !newItems && !deletedItems;
        }

        /// <summary>
        /// Selects a random entry from a list
        /// </summary>
        /// <typeparam name="T">The type of entry</typeparam>
        /// <param name="enumerable">The list</param>
        /// <returns>A random entry</returns>
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            int index = new Random().Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
}
