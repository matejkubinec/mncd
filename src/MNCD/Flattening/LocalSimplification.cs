using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Flattening
{
    public class LocalSimplification
    {
        public Network BasedOnLayerRelevance(Network network, double[] layerRelevances, double threshold)
        {
            if (network.Layers.Count != layerRelevances.Length)
            {
                throw new ArgumentException("Relevances count doesn't match the layers count.");
            }

            var flattenedNetwork = new Network()
            {
                Actors = network.Actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Flattened"
                    }
                }
            };

            for (var i = 0; i < network.Layers.Count; i++)
            {
                if (layerRelevances[i] >= threshold)
                {
                    flattenedNetwork.Layers[0].Edges = flattenedNetwork.Layers[0].Edges.Concat(network.Layers[i].Edges).ToList();
                }
            }

            return flattenedNetwork;
        }
    }
}
