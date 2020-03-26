using MNCD.Core;
using MNCD.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.CommunityDetection.MultiLayer
{
    /// <summary>
    /// Implementation of ABACUS community detection algorithm.
    /// </summary>
    public class ABACUS
    {
        /// <summary>
        /// Applies ABACUS community detection algorithm on supplied network.
        /// ABACUS: frequent pAttern mining-BAsed Community discovery in mUltidimensional networkS
        /// Michele Berlingerio, Fabio Pinelli, Francesco Calabrese
        /// https://arxiv.org/pdf/1303.2025.pdf.
        /// </summary>
        /// <param name="network">Multi-layer network.</param>
        /// <param name="cd">Community detection algorithm.</param>
        /// <param name="treshold">Treshold for frequent itemset mining.</param>
        /// <returns>List of overlapping communities.</returns>
        public List<Community> Apply(
            Network network,
            Func<Network, List<Community>> cd,
            double treshold)
        {
            var membership = network.Actors
                .ToDictionary(a => a, a => new HashSet<(int, int)>());
            var layerNetworks = network.Layers
                .Select(l => LayerToNetwork(l));
            var layerCommunities = new List<List<Community>>();

            // 1. Each dimension is treated separetely
            // and monodimensional communities are extracted
            for (var i = 0; i < network.LayerCount; i++)
            {
                var communities = cd(layerNetworks.ElementAt(i));
                layerCommunities.Add(communities);
                for (var j = 0; j < communities.Count; j++)
                {
                    foreach (var actor in communities[j].Actors)
                    {
                        // 2. Each node is labeled with a list of pairs
                        // (dimension, community the node belongs to in that dimension)
                        membership[actor].Add((i, j));
                    }
                }
            }

            // 3. Each pair is treated as an item and a frequent closed itemset
            // mining algorithm is applied
            var itemSets = new Apriori(membership, treshold).GetClosedItemSets();
            var result = BuildCommunities(membership, itemSets);

            // Add actors without communities into their own communities
            foreach (var actor in network.Actors)
            {
                if (result.All(c => !c.Actors.Contains(actor)))
                {
                    result.Add(new Community(actor));
                }
            }

            return result;
        }

        private List<Community> BuildCommunities(
            Dictionary<Actor, HashSet<(int, int)>> membership,
            List<HashSet<(int, int)>> itemSets)
        {
            var communities = new List<Community>();
            foreach (var itemSet in itemSets)
            {
                var actors = membership
                    .Where(m => itemSet.IsSubsetOf(m.Value))
                    .Select(m => m.Key);
                var community = new Community(actors);
                communities.Add(community);
            }

            return communities;
        }

        private Network LayerToNetwork(Layer layer)
        {
            return new Network
            {
                Layers = new List<Layer> { layer },
                Actors = layer.GetLayerActors(),
            };
        }

        /// <summary>
        /// This class implements the apriori algorithm based on:
        /// https://www.geeksforgeeks.org/apriori-algorithm/.
        /// </summary>
        private class Apriori
        {
            private readonly Dictionary<Actor, HashSet<(int, int)>> _membership;
            private readonly double _treshold;
            private readonly List<HashSet<(int, int)>> _closedItemSets;

            public Apriori(
                Dictionary<Actor, HashSet<(int, int)>> membership,
                double treshold)
            {
                _membership = membership;
                _treshold = treshold;
                _closedItemSets = new List<HashSet<(int, int)>>();
            }

            public List<HashSet<(int, int)>> GetClosedItemSets()
            {
                var items = _membership.Values
                    .SelectMany(v => v)
                    .ToHashSet()
                    .Select(v => new HashSet<(int, int)> { v })
                    .ToList();

                Step(items, 1);

                return _closedItemSets;
            }

            private void Step(List<HashSet<(int, int)>> itemsets, int k)
            {
                var support = GetSupport(itemsets);
                var supported = support
                    .Where(s => s.Value >= _treshold)
                    .Select(s => s.Key)
                    .ToList();
                var joined = Join(supported, k + 1);
                var joinedSupport = GetSupport(joined);

                // Find closed itemsets
                foreach (var subset in supported)
                {
                    foreach (var superset in joined)
                    {
                        if (subset.IsSubsetOf(superset) && joinedSupport[superset] == support[subset])
                        {
                            continue;
                        }

                        _closedItemSets.Add(subset);
                    }
                }

                if (joined.Count != 0)
                {
                    Step(joined, k + 1);
                }
            }

            private Dictionary<HashSet<(int, int)>, int> GetSupport(List<HashSet<(int, int)>> itemsets)
            {
                var support = itemsets.ToDictionary(i => i, i => 0);
                var membership = _membership.Values;
                foreach (var itemset in itemsets)
                {
                    support[itemset] += membership
                        .Where(m => itemset.IsSubsetOf(m))
                        .Count();
                }

                return support;
            }

            private List<HashSet<(int, int)>> Join(List<HashSet<(int, int)>> itemsests, int k)
            {
                var joined = new List<HashSet<(int, int)>>();
                foreach (var i1 in itemsests)
                {
                    foreach (var i2 in itemsests)
                    {
                        if (i1.Intersect(i2).Count() >= (k - 2))
                        {
                            var concatenated = i1.Concat(i2).ToHashSet();

                            if (!joined.Any(j => j.SetEquals(concatenated)))
                            {
                                joined.Add(concatenated);
                            }
                        }
                    }
                }

                return joined
                    .Where(j => j.Count >= k)
                    .Select(j => j.OrderBy(s => s.Item1).ThenBy(s => s.Item2).ToHashSet())
                    .ToHashSet()
                    .Distinct()
                    .ToList();
            }
        }
    }
}