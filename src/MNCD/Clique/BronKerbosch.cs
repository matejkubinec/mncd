using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Clique
{
    /// <summary>
    /// Implements Bron Kerbosch algorithm.
    /// </summary>
    public class BronKerbosch
    {
        private List<List<Actor>> MaximalCliques { get; set; }

        private List<Actor> Nodes { get; set; }

        private Dictionary<Actor, List<Actor>> Neighbours { get; set; }

        /// <summary>
        /// Gets maximal cliques in network.
        /// </summary>
        /// <param name="network">SingleLayer network.</param>
        /// <returns>Maximal cliques.</returns>
        public List<List<Actor>> GetMaximalCliques(Network network)
        {
            if (network.LayerCount > 1)
            {
                throw new ArgumentException("Algorithm works only on single layer networks.");
            }

            MaximalCliques = new List<List<Actor>>();
            Nodes = InitNodes(network);
            Neighbours = InitNeighbours(network);

            Compute(new List<Actor>(), Nodes, new List<Actor>());

            return MaximalCliques;
        }

        /// <summary>
        /// Computes maximal cliques recursively.
        /// </summary>
        /// <param name="r">Currently growing clique.</param>
        /// <param name="p">Prospective nodes connected to all nodes in R.</param>
        /// <param name="x">Already processed nodes.</param>
        internal void Compute(
            IEnumerable<Actor> r,
            IEnumerable<Actor> p,
            IEnumerable<Actor> x)
        {
            if (p.Count() == 0 && x.Count() == 0)
            {
                MaximalCliques.Add(r.ToList());
            }
            else
            {
                foreach (var v in p)
                {
                    Compute(
                        r.Append(v),
                        p.Intersect(Neighbours[v]),
                        x.Intersect(Neighbours[v]));

                    p = p.Where(p => p != v);
                    x = x.Append(v);
                }
            }
        }

        /// <summary>
        /// Get actors from network.
        /// </summary>
        /// <param name="network">Network.</param>
        /// <returns>Actors from network.</returns>
        internal List<Actor> InitNodes(Network network)
        {
            return network.Actors.ToList();
        }

        /// <summary>
        /// Initializes neighbours dictionary.
        /// </summary>
        /// <param name="network">Network.</param>
        /// <returns>Dictionary from actors to its neighbours.</returns>
        internal Dictionary<Actor, List<Actor>> InitNeighbours(Network network)
        {
            var neighbours = new Dictionary<Actor, List<Actor>>();

            foreach (var actor in network.Actors)
            {
                neighbours.Add(actor, new List<Actor>());
            }

            foreach (var edge in network.Layers[0].Edges)
            {
                neighbours[edge.From].Add(edge.To);
                neighbours[edge.To].Add(edge.From);
            }

            return neighbours;
        }
    }
}
