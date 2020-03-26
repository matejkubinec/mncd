using System.Collections.Generic;
using System.Linq;

namespace MNCD.Core
{
    /// <summary>
    /// Class representing a layer.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        public Layer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        /// <param name="name">Name of the layer.</param>
        public Layer(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        /// <param name="edges">List of edges that should be in the layer.</param>
        public Layer(List<Edge> edges)
        {
            Edges = edges;
        }

        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of edges in the layer.
        /// </summary>
        public List<Edge> Edges { get; set; } = new List<Edge>();

        /// <summary>
        /// Gets actors from edges in layers.
        /// </summary>
        /// <returns>List of actors.</returns>
        public List<Actor> GetLayerActors()
        {
            var actors = new HashSet<Actor>();
            foreach (var edge in Edges)
            {
                actors.Add(edge.From);
                actors.Add(edge.To);
            }

            return actors.ToList();
        }

        /// <summary>
        /// Creates actor to neigbours dictionary.
        /// </summary>
        /// <returns>Dictionary of actors an theirs neighbours.</returns>
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