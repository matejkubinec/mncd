using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Evaluation.MultiLayer
{
    /// <summary>
    /// Class implementing homogenity measure.
    /// Finding Redundant and Complementary Communities in Multidimensional Networks
    /// http://www.michelecoscia.com/wp-content/uploads/2012/08/cosciacikm11.pdf
    /// Michele Berlingerio, Michele Coscia, Fosca Giannotti.
    /// </summary>
    public static class Homogenity
    {
        /// <summary>
        /// Computes homogenity for supplied community in supplied network.
        /// </summary>
        /// <param name="community">Community for which homogenity will be computed.</param>
        /// <param name="network">Network in which community resides.</param>
        /// <returns>Homogenity value for community.</returns>
        public static double Compute(Community community, Network network)
        {
            if (network.LayerCount <= 1)
            {
                throw new ArgumentException("Homogenity can be computed only for multi-layered networkx.");
            }

            if (community.Size == 0)
            {
                return 1;
            }

            var d = network.Layers.Count;
            var edgeLayerCounts = new List<double>();

            foreach (var layer in network.Layers)
            {
                var edgesInLayer = 0;

                foreach (var edge in layer.Edges)
                {
                    if (community.Actors.Contains(edge.From) &&
                        community.Actors.Contains(edge.To))
                    {
                        edgesInLayer++;
                    }
                }

                edgeLayerCounts.Add(edgesInLayer);
            }

            var sigmaC = GetSigmaC(edgeLayerCounts, d);
            var sigmaCMax = GetSigmaCMax(edgeLayerCounts);

            if (sigmaCMax == 0)
            {
                return 1;
            }

            return 1 - (sigmaC / sigmaCMax);
        }

        private static double GetSigmaC(List<double> pcds, int d)
        {
            var avgC = pcds.Average();
            var res = 0.0;
            foreach (var pcd in pcds)
            {
                res += Math.Pow(pcd - avgC, 2);
            }

            return Math.Sqrt(res);
        }

        private static double GetSigmaCMax(List<double> pcds)
        {
            return Math.Sqrt(Math.Pow(pcds.Max() - 1, 2) / 2.0);
        }
    }
}