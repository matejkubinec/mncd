using System.Collections.Generic;
using System.Linq;

namespace MNCD.Core
{
    public class Layer
    {
        public string Name { get; set; }
        public List<Edge> Edges = new List<Edge>();

        public Layer()
        {
        }

        public Layer(string name)
        {
            Name = name;
        }

        public Layer(List<Edge> edges)
        {
            Edges = edges;
        }

        public List<Actor> GetActors()
        {
            var actors = new HashSet<Actor>();
            foreach (var edge in Edges)
            {
                actors.Add(edge.From);
                actors.Add(edge.To);
            }
            return actors.ToList();
        }

        public Dictionary<Actor, List<Actor>> GetNeighboursDict()
        {
            var dict = new Dictionary<Actor, HashSet<Actor>>();
            foreach (var edge in Edges)
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