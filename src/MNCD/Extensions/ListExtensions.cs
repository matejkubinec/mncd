using System.Collections.Generic;
using System.Linq;

namespace MNCD.Extensions
{
    public static class ListExtensions
    {
        public static Dictionary<T, int> ToIndexDict<T>(this List<T> list)
        {
            return list
                .Select((item, index) => (item, index))
                .ToDictionary(pair => pair.item, pair => pair.index);
        }
    }
}