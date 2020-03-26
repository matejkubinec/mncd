using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Extensions
{
    /// <summary>
    /// List extensions.
    /// </summary>
    public static class ListExtensions
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Creates dictionary that gets index based on list item.
        /// </summary>
        /// <typeparam name="T">Type of list item.</typeparam>
        /// <param name="list">List of items.</param>
        /// <returns>Dictionary of item and its index.</returns>
        public static Dictionary<T, int> ToIndexDict<T>(this List<T> list) => list
            .Select((item, index) => (item, index))
            .ToDictionary(pair => pair.item, pair => pair.index);

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Dictionary<T, int> ToIndexDictionary<T>(this List<T> items) => items
                .Select((item, index) => (item, index))
                .ToDictionary(pair => pair.item, pair => pair.index);
    }
}