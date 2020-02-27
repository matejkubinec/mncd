using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Clique;
using MNCD.Components;
using MNCD.Core;


namespace MNCD.CommunityDetection.SingleLayer
{
    public class KClique
    {
        /// <summary>
        /// Find k-clique communities in network using the percolation method.
        /// 
        /// A k-clique community is the union of all cliques of size k that 
        /// can be reached through adjacent (sharing k-1 nodes) k-cliques.
        /// </summary>
        /// <param name="network"></param>
        /// <param name="k">Size of smallest clique</param>
        /// <returns></returns>
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


        internal Network GetPercolationNetwork(
            List<List<Actor>> cliques,
            Dictionary<Actor, List<List<Actor>>> membership,
            Dictionary<List<Actor>, Actor> cliqueToActor,
            int k)
        {
            var percNetwork = new Network();
            percNetwork.Actors = cliques.Select(c => cliqueToActor[c]).ToList();
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

        internal Dictionary<Actor, List<Actor>> GetActorToClique(Dictionary<List<Actor>, Actor> cliqueToActor)
        {
            var dict = new Dictionary<Actor, List<Actor>>();
            foreach (var pair in cliqueToActor)
            {
                dict.Add(pair.Value, pair.Key);
            }
            return dict;
        }

        internal Dictionary<List<Actor>, Actor> GetCliqueToActor(List<List<Actor>> cliques)
        {
            var i = 0;
            return cliques.ToDictionary(c => c, c => new Actor("c" + i++));
        }

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
                            c
                        };
                    }
                }
            }
            return membership;
        }
    }
}