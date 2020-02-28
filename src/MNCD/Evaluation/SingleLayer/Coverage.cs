using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Evaluation.SingleLayer
{
    // https://networkx.github.io/documentation/stable/reference/algorithms/generated/networkx.algorithms.community.quality.coverage.html#networkx.algorithms.community.quality.coverage
    public static class Coverage
    {
        public static double Get(Network network, List<Community> communities)
        {
            if (network.LayerCount != 1) throw new ArgumentException("Coverage only works on single layer networks.");


            var intra = GetIntraEdges(network, communities);
            var total = GetTotalEdges(network);

            if (total > 0)
            {
                return intra / (double)total;
            }
            else
            {
                return 1.0;
            }
        }

        private static int GetIntraEdges(Network network, List<Community> communities)
        {
            var count = 0;
            foreach (var edge in network.Layers.First().Edges)
            {
                if (communities.Any(c => c.Actors.Contains(edge.From) && c.Actors.Contains(edge.To)))
                {
                    count++;
                }
            }
            return count;
        }

        private static int GetTotalEdges(Network network)
        {
            return network.Layers.First().Edges.Count;
        }
    }
}