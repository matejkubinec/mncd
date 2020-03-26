using MNCD.Core;
using MNCD.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Flattening
{
    public class WeightedFlattening
    {
        public Network Flatten(Network network, double[,] weights)
        {
            var flattened = new Network(new Layer(), network.Actors);
            var layerToIndex = network.GetLayerToIndex();
            var edgesDict = new Dictionary<(Actor from, Actor to), double>();

            foreach (var layer in network.Layers)
            {
                var idx = layerToIndex[layer];
                foreach (var edge in layer.Edges)
                {
                    if (edgesDict.ContainsKey((edge.From, edge.To)))
                    {
                        edgesDict[(edge.From, edge.To)] += edge.Weight * weights[idx, idx];
                    }
                    else if (edgesDict.ContainsKey((edge.To, edge.From)))
                    {
                        edgesDict[(edge.To, edge.From)] += edge.Weight * weights[idx, idx];
                    }
                    else
                    {
                        edgesDict[(edge.From, edge.To)] = edge.Weight * weights[idx, idx];
                    }
                }
            }

            foreach (var edge in network.InterLayerEdges)
            {
                var lf = layerToIndex[edge.LayerFrom];
                var lt = layerToIndex[edge.LayerTo];

                if (edgesDict.ContainsKey((edge.From, edge.To)))
                {
                    edgesDict[(edge.From, edge.To)] += edge.Weight * weights[lf, lt];
                }
                else if (edgesDict.ContainsKey((edge.To, edge.From)))
                {
                    edgesDict[(edge.To, edge.From)] += edge.Weight * weights[lf, lt];
                }
                else
                {
                    edgesDict[(edge.From, edge.To)] = edge.Weight * weights[lf, lt];
                }
            }

            flattened.FirstLayer.Edges = edgesDict
                .Select(e => new Edge(e.Key.from, e.Key.to, e.Value))
                .ToList();

            return flattened;
        }
    }
}
