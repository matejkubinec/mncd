using System;
using System.Collections.Generic;
using MNCD.Core;

namespace MNCD.Evaluation.MultiLayer
{
    /// <summary>
    /// Implements exclusivity measure.
    /// Based on:
    /// Finding Redundant and Complementary Communities in Multidimensional Networks
    /// http://www.michelecoscia.com/wp-content/uploads/2012/08/cosciacikm11.pdf
    /// Michele Berlingerio, Michele Coscia, Fosca Giannotti.
    /// </summary>
    public static class Exclusivity
    {
        /// <summary>
        /// Exclusivity can be computed as the ratio between the
        /// number of exclusive connections within the community and
        /// the total number of connected pairs in c.
        /// </summary>
        /// <param name="community">Community for which the exclusivity should be computed.</param>
        /// <param name="network">Network in which the community resides.</param>
        /// <returns>Exclusivity of community in network.</returns>
        public static double Compute(Community community, Network network)
        {
            if (network.LayerCount <= 1)
            {
                throw new ArgumentException("Exclusivity can be computed only for multi-layered networkx.");
            }

            if (community.Size == 0)
            {
                return 0;
            }

            var pairToLayers = new Dictionary<ValueTuple<Actor, Actor>, HashSet<Layer>>();

            foreach (var layer in network.Layers)
            {
                foreach (var edge in layer.Edges)
                {
                    if (community.Actors.Contains(edge.From) &&
                        community.Actors.Contains(edge.To))
                    {
                        var pair = (edge.From, edge.To);

                        if (pairToLayers.ContainsKey(pair))
                        {
                            pairToLayers[pair].Add(layer);
                        }
                        else
                        {
                            pairToLayers[pair] = new HashSet<Layer>
                            {
                                layer,
                            };
                        }
                    }
                }
            }

            var exclusiveConnections = 0;
            var totalConnections = 0;

            foreach (var layers in pairToLayers.Values)
            {
                if (layers.Count == 1)
                {
                    exclusiveConnections++;
                }

                totalConnections += layers.Count;
            }

            return exclusiveConnections / totalConnections;
        }
    }
}