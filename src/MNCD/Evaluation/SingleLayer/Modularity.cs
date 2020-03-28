using MNCD.Core;
using MNCD.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Evaluation.SingleLayer
{
    /// <summary>
    /// Class that implements modularity measure.
    /// Based on: http://networksciencebook.com/chapter/9#modularity.
    /// </summary>
    public static class Modularity
    {
        /// <summary>
        /// Computes modularity for network patitioning.
        /// </summary>
        /// <param name="network">
        /// Network that is partitioned.
        /// </param>
        /// <param name="communities">
        /// List of communities for which the modularity should be computed.
        /// </param>
        /// <returns>Modularity of the partition.</returns>
        public static double Compute(Network network, List<Community> communities)
        {
            var edges = network.FirstLayer.Edges;
            var l = (double)edges.Count();
            var lc = CommunityToLinkCount(edges, communities);
            var kc = CommunityToDegrees(network, communities);

            var m = 0.0;
            foreach (var c in communities)
            {
                m += (lc[c] / l) - Math.Pow(kc[c] / (2.0 * l), 2.0);
            }

            return m;
        }

        private static Dictionary<Community, int> CommunityToLinkCount(
            List<Edge> edges,
            List<Community> communities)
        {
            var res = communities.ToDictionary(c => c, c => 0);
            foreach (var edge in edges)
            {
                foreach (var c in communities)
                {
                    if (c.Actors.Contains(edge.From) &&
                        c.Actors.Contains(edge.To))
                    {
                        res[c]++;
                    }
                }
            }

            return res;
        }

        private static Dictionary<Community, int> CommunityToDegrees(
            Network network,
            List<Community> communities)
        {
            var atd = network.GetActorToDegree();
            var res = communities.ToDictionary(c => c, c => 0);
            foreach (var c in communities)
            {
                foreach (var a in c.Actors)
                {
                    if (atd.ContainsKey(a))
                    {
                        res[c] += (int)atd[a];
                    }
                }
            }

            return res;
        }
    }
}
