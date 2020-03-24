using MNCD.Core;
using System;

namespace MNCD.Evaluation.MultiLayer
{
    /// <summary>
    /// Implements variety.
    /// Based on:
    /// Finding Redundant and Complementary Communities in Multidimensional Networks
    /// http://www.michelecoscia.com/wp-content/uploads/2012/08/cosciacikm11.pdf
    /// Michele Berlingerio, Michele Coscia, Fosca Giannotti.
    /// </summary>
    public static class Variety
    {
        /// <summary>
        /// Variety - number of dimensions expressed with the community
        /// over the total number of dimensions within the network.
        /// </summary>
        /// <param name="community">Community for which variety is computed.</param>
        /// <param name="network">Network in which community resides.</param>
        /// <returns>Variety for community in network.</returns>
        public static double Compute(Community community, Network network)
        {
            if (network.LayerCount <= 1)
            {
                throw new ArgumentException("Variety can be computed only for multi-layered networkx.");
            }

            if (community.Size == 0)
            {
                return 0;
            }

            var d = network.Layers.Count;
            var dc = 0;

            var actors = community.Actors;
            foreach (var layer in network.Layers)
            {
                foreach (var edge in layer.Edges)
                {
                    if (actors.Contains(edge.From) && actors.Contains(edge.To))
                    {
                        dc++;
                        break;
                    }
                }
            }

            return (dc - 1) / (d - 1);
        }
    }
}