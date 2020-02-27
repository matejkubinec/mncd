using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Clique
{
    public class BronKerbosch
    {
        private List<List<Actor>> MaximalCliques { get; set; }
        private List<Actor> Nodes { get; set; }
        private Dictionary<Actor, List<Actor>> Neighbours { get; set; }

        public List<List<Actor>> GetMaximalCliques(Network network)
        {
            if (network.LayerCount > 1)
            {
                throw new ArgumentException("Algorithm works only on single layer networks.");
            }

            if (network.Layers[0].IsDirected)
            {
                throw new ArgumentException("Algorithm works only on nondirected networks.");
            }

            MaximalCliques = new List<List<Actor>>();
            Nodes = InitNodes(network);
            Neighbours = InitNeighbours(network);

            Compute(new List<Actor>(), Nodes, new List<Actor>());

            return MaximalCliques;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R">Currently growing clique</param>
        /// <param name="P">Prospective nodes connected to all nodes in R</param>
        /// <param name="X">Already processed nodes</param>
        internal void Compute(
            IEnumerable<Actor> R,
            IEnumerable<Actor> P,
            IEnumerable<Actor> X)
        {
            if (P.Count() == 0 && X.Count() == 0)
            {
                MaximalCliques.Add(R.ToList());
            }
            else
            {
                foreach (var v in P)
                {
                    Compute(
                        R.Append(v),
                        P.Intersect(Neighbours[v]),
                        X.Intersect(Neighbours[v]));

                    P = P.Where(p => p != v);
                    X = X.Append(v);
                }
            }
        }

        internal List<Actor> InitNodes(Network network)
        {
            return network.Actors.ToList();
        }

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
