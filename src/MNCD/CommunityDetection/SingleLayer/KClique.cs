using MNCD.Clique;
using MNCD.Components;
using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.CommunityDetection.SingleLayer
{
    /// <summary>
    /// Implements KClique community detection algorithm.
    /// </summary>
    public class KClique
    {
        /// <summary>
        /// Find k-clique communities in network using the percolation method.
        ///
        /// A k-clique community is the union of all cliques of size k that
        /// can be reached through adjacent (sharing k-1 nodes) k-cliques.
        /// </summary>
        /// <param name="network">Network in which to find communities.</param>
        /// <param name="k">Size of smallest clique.</param>
        /// <returns>List of communities.</returns>
        public List<Community> GetKCommunities(Network network, int k)
        {
            if (k < 2)
            {
                throw new ArgumentException("K must be greater than 1.");
            }

            var cliques = new BronKerbosch()
                .GetMaximalCliques(network)
                .Where(c => c.Count >= k)
                .ToList();

            var membership = AssignMembership(cliques);
            var cliqueToActor = GetCliqueToActor(cliques);
            var actorToClique = GetActorToClique(cliqueToActor);
            var percNetwork = GetPercolationNetwork(cliques, membership, cliqueToActor, k);

            return percNetwork.Layers
                .First()
                .GetConnectedComponents(percNetwork.Actors)
                .Select(c => new Community(c.SelectMany(a => actorToClique[a])))
                .ToList();
        }

        /// <summary>
        /// Gets percolation network.
        /// </summary>
        /// <param name="cliques">Cliques.</param>
        /// <param name="membership">Membership.</param>
        /// <param name="cliqueToActor">Mapping of cliques to actors.</param>
        /// <param name="k">Size of smallest clique.</param>
        /// <returns>Percolation network.</returns>
        internal Network GetPercolationNetwork(
            List<List<Actor>> cliques,
            Dictionary<Actor, List<List<Actor>>> membership,
            Dictionary<List<Actor>, Actor> cliqueToActor,
            int k)
        {
            var percNetwork = new Network
            {
                Actors = cliques.Select(c => cliqueToActor[c]).ToList(),
            };
            percNetwork.Layers.Add(new Layer());

            var edges = new List<Edge>();
            foreach (var clique in cliques)
            {
                foreach (var adjacentClique in GetAdjacentCliques(clique, membership))
                {
                    if (clique.Intersect(adjacentClique).Count() >= (k - 1))
                    {
                        var from = cliqueToActor[clique];
                        var to = cliqueToActor[adjacentClique];
                        var edge = new Edge(from, to);

                        if (!edges.Any(e => (e.From == from && e.To == to) ||
                                            (e.From == to && e.To == from)))
                        {
                            edges.Add(edge);
                        }
                    }
                }
            }

            percNetwork.Layers[0].Edges = edges;

            return percNetwork;
        }

        /// <summary>
        /// Creates actor to clique membership.
        /// </summary>
        /// <param name="cliqueToActor">Clique to actor membership.</param>
        /// <returns>Mapping of actor to a clique.</returns>
        internal Dictionary<Actor, List<Actor>> GetActorToClique(
            Dictionary<List<Actor>, Actor> cliqueToActor)
        {
            var dict = new Dictionary<Actor, List<Actor>>();
            foreach (var pair in cliqueToActor)
            {
                dict.Add(pair.Value, pair.Key);
            }

            return dict;
        }

        /// <summary>
        /// Returns mapping of clique to an actor.
        /// </summary>
        /// <param name="cliques">List of cliques.</param>
        /// <returns>Mapping of clique to an actor.</returns>
        internal Dictionary<List<Actor>, Actor> GetCliqueToActor(List<List<Actor>> cliques)
        {
            var i = 0;
            return cliques.ToDictionary(c => c, c =>
            {
                var id = i++;
                return new Actor(id, "c" + id);
            });
        }

        /// <summary>
        /// Computes adjacent cliques for supplied clique.
        /// </summary>
        /// <param name="clique">Clique for which adjacent cliques should be found.</param>
        /// <param name="membership">Membership.</param>
        /// <returns>List of adjacent cliques.</returns>
        internal List<List<Actor>> GetAdjacentCliques(
            List<Actor> clique,
            Dictionary<Actor, List<List<Actor>>> membership)
        {
            var adjacent = new HashSet<List<Actor>>();
            foreach (var actor in clique)
            {
                foreach (var adjacentClique in membership[actor])
                {
                    if (adjacentClique != clique)
                    {
                        adjacent.Add(adjacentClique);
                    }
                }
            }

            return adjacent.ToList();
        }

        /// <summary>
        /// Creates membership of actor to clique.
        /// </summary>
        /// <param name="cliques">List of cliques.</param>
        /// <returns>Dictionary of actors and cliques.</returns>
        internal Dictionary<Actor, List<List<Actor>>> AssignMembership(IEnumerable<List<Actor>> cliques)
        {
            var membership = new Dictionary<Actor, List<List<Actor>>>();
            foreach (var c in cliques)
            {
                foreach (var a in c)
                {
                    if (membership.ContainsKey(a))
                    {
                        membership[a].Add(c);
                    }
                    else
                    {
                        membership[a] = new List<List<Actor>>
                        {
                            c,
                        };
                    }
                }
            }

            return membership;
        }
    }
}