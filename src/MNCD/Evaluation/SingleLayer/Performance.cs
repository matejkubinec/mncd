using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Evaluation.SingleLayer
{
    /// <summary>
    /// Implement Performance quality function for communities.
    /// Based on:
    /// Community detection in graphs
    /// https://arxiv.org/abs/0906.0612
    /// Santo Fortunato.
    /// </summary>
    public static class Performance
    {
        /// <summary>
        /// Get perfomance for partition of network.
        /// </summary>
        /// <param name="network">
        /// Network that is partitioned.
        /// </param>
        /// <param name="communities">
        /// List of communities for which the performance should be computed.
        /// </param>
        /// <returns>
        /// Performance of a patitioning of network.
        /// </returns>
        public static double Get(Network network, List<Community> communities)
        {
            var intra = GetIntraEdges(network, communities);
            var inter = GetInterEdges(network, communities);
            var n = network.Actors.Count;
            var totalPairs = (n * (n - 1)) / 2.0;

            if (totalPairs > 0)
            {
                return (intra + inter) / totalPairs;
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

        private static int GetInterEdges(Network network, List<Community> communities)
        {
            var interEdges = 0;
            for (var i = 0; i < communities.Count; i++)
            {
                for (var j = i + 1; j < communities.Count; j++)
                {
                    var c1 = communities[i];
                    var c2 = communities[j];

                    var maximalCount = c1.Size * c2.Size;

                    foreach (var edge in network.FirstLayer.Edges)
                    {
                        if ((c1.Contains(edge.From) && c2.Contains(edge.To)) ||
                            (c2.Contains(edge.From) && c1.Contains(edge.To)))
                        {
                            maximalCount--;
                        }
                    }

                    interEdges += maximalCount;
                }
            }

            return interEdges;
        }
    }
}