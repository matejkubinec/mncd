using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Tests.Helpers
{
    public static class ActorHelper
    {
        public static List<Actor> ActorsFrom(params string[] names)
        {
            return names.Select(n => new Actor(n)).ToList();
        }

        public static List<Actor> Get(int count)
        {
            return Enumerable
                .Range(0, count)
                .Select(n => new Actor("a" + n))
                .ToList();
        }
    }
}