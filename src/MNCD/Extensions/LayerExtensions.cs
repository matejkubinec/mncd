using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Extensions
{
    public static class LayerExtensions
    {
        public static List<Actor> GetActors(this Layer layer)
        {
            var actors = new HashSet<Actor>();
            foreach (var edge in layer.Edges)
            {
                actors.Add(edge.From);
                actors.Add(edge.To);
            }
            return actors.ToList();
        }

        public static Dictionary<Actor, List<Actor>> GetNeighboursDict(this Layer layer)
        {
            var dict = new Dictionary<Actor, HashSet<Actor>>();
            foreach (var edge in layer.Edges)
            {
                if (!dict.ContainsKey(edge.From))
                {
                    dict[edge.From] = new HashSet<Actor>();
                }

                if (!dict.ContainsKey(edge.To))
                {
                    dict[edge.To] = new HashSet<Actor>();
                }

                dict[edge.From].Add(edge.To);
                dict[edge.To].Add(edge.From);
            }
            return dict.ToDictionary(d => d.Key, d => d.Value.ToList());
        }
    }
}