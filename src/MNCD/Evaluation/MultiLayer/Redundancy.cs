using MNCD.Core;
using System;
using System.Linq;

namespace MNCD.Evaluation.MultiLayer
{
    /// <summary>
    /// Implement redundancy measure.
    /// 2.3 Redundancy
    /// Finding Redundant and Complementary Communities in Multidimensional Networks
    /// http://www.michelecoscia.com/wp-content/uploads/2012/08/cosciacikm11.pdf
    /// Michele Berlingerio, Michele Coscia, Fosca Giannotti.
    /// </summary>
    public static class Redundancy
    {
        /// <summary>
        /// Computes redundancy measure for community.
        /// </summary>
        /// <param name="community">Community for which the redundancy should be computed.</param>
        /// <param name="network">Network in which the community resides.</param>
        /// <returns>Redundancy.</returns>
        public static double Compute(Community community, Network network)
        {
            if (network.LayerCount <= 1)
            {
                throw new ArgumentException("Redundancy can be computed only for multi-layered networkx.");
            }

            if (community.Size == 0)
            {
                return 0;
            }

            var d = network.LayerCount;

            var edges = network.Layers
                .SelectMany(l => l.Edges)
                .Select(e => e.Pair)
                .Distinct();

            var edgeLayer = edges.ToDictionary(e => e, e => 0);

            var pc = edges.Count();

            foreach (var edge in edges)
            {
                foreach (var layer in network.Layers)
                {
                    if (layer.Edges.Any(e => e.Pair == edge || e.Reverse().Pair == edge))
                    {
                        edgeLayer[edge]++;
                    }
                }
            }

            return edgeLayer.Values
                .Where(v => v > 1)
                .Sum(v => v / (double)(d * pc));
        }
    }
}
