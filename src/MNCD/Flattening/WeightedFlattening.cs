using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Flattening
{
    public class WeightedFlattening
    {
        // TODO: implement - Multilayer Social Networks - page 74 - Weighted Flattening
        public Network Flatten(Network network, Dictionary<(Layer, Layer), double> weights)
        {
            var flattened = new Network(new Layer(), network.Actors);
            var edgesDict = new Dictionary<(Actor from, Actor to), double>();

            foreach(var layer in network.Layers)
            {
                foreach(var edge in layer.Edges)
                {
                    if (edgesDict.ContainsKey((edge.From, edge.To)))
                    {
                        edgesDict[(edge.From, edge.To)] += edge.Weight * weights[(layer, layer)];
                    }
                    else if (edgesDict.ContainsKey((edge.To, edge.From)))
                    {
                         edgesDict[(edge.To, edge.From)] += edge.Weight * weights[(layer, layer)];
                    }
                    else
                    {
                        edgesDict[(edge.From, edge.To)] = edge.Weight * weights[(layer, layer)];
                    }
                }
            }

            foreach(var edge in network.InterLayerEdges)
            {
                var lf = edge.LayerFrom;
                var lt = edge.LayerTo;
                var weight = weights.ContainsKey((lf, lt)) ? weights[(lf, lt)] : weights[(lt, lf)];

                if (edgesDict.ContainsKey((edge.From, edge.To)))
                {
                    edgesDict[(edge.From, edge.To)] += edge.Weight * weight;
                }
                else if (edgesDict.ContainsKey((edge.To, edge.From)))
                {
                        edgesDict[(edge.To, edge.From)] += edge.Weight * weight;
                }
                else
                {
                    edgesDict[(edge.From, edge.To)] = edge.Weight * weight;
                }
            }
            flattened.FirstLayer.Edges = edgesDict
                .Select(e => new Edge(e.Key.from, e.Key.to, e.Value))
                .ToList();
            return flattened;
        }
    }
}
