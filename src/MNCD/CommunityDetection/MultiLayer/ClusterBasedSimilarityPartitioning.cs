using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Clustering;
using MNCD.CommunityDetection.SingleLayer;
using MNCD.Core;

namespace MNCD.CommunityDetection.MultiLayer
{
    public class ClusterBasedSimilarityPartitioning
    {
        private static Random Random = new Random();

        public List<Community> GetCommunities(
            Network network,
            Func<Network, List<Community>> cd,
            int k)
        {
            var networks = network.Layers.Select(l => l.ToNetwork());
            var communities = new List<Community>();

            foreach (var n in networks)
            {
                var c = new Louvain().Apply(n);
                communities.AddRange(c);
            }

            var similarities = GetSimilarityDict(network, communities);
            var kMedoids = new KMedoids<Actor>(network.Actors, similarities);
            var clusterized = kMedoids.Clusterize(k);

            return clusterized.Select(c => new Community(c)).ToList();
        }

        private Dictionary<(Actor, Actor), double> GetSimilarityDict(
            Network network,
            List<Community> communities)
        {
            var similarities = new Dictionary<(Actor, Actor), double>();
            var actors = network.Actors;
            for (var i = 0; i < actors.Count; i++)
            {
                var a1 = actors[i];
                similarities.Add((a1, a1), 1.0);

                for (var j = i + 1; j < actors.Count; j++)
                {
                    var a2 = actors[j];

                    var count = 0.0;
                    foreach (var c in communities)
                    {
                        if (c.Actors.Contains(a1) && c.Actors.Contains(a2))
                        {
                            count++;
                        }
                    }

                    var similarity = count / (double)network.LayerCount;
                    similarities.Add((a1, a2), similarity);
                    similarities.Add((a2, a1), similarity);
                }
            }

            return similarities;
        }
    }
}