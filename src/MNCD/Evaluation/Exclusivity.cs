using System;
using System.Collections.Generic;
using MNCD.Core;

namespace MNCD.Evaluation
{
    // 
    // Exclusivity can be computed as the ratio between the 
    // number of exclusive connections within the community and
    // the total number of connected pairs in c:
    // 
    // exclusive connection - every pair of nodes in c is connected by at least
    // two dimensions
    public static class Exclusivity
    {
        public static double Compute(Community community, Network network)
        {
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
                                layer
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