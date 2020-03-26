using MNCD.Core;
using System.Linq;

namespace MNCD.Flattening
{
    public class MergeFlattening
    {
        public Network Merge(Network network, bool includeWeights)
        {
            var flattenedNetwork = new Network(new Layer("Flattened"), network.Actors);
            var edges = flattenedNetwork.FirstLayer.Edges;

            foreach (var layer in network.Layers)
            {
                foreach (var layerEdge in layer.Edges)
                {
                    var edge = edges.FirstOrDefault(e =>
                        (e.From == layerEdge.From && e.To == layerEdge.To) ||
                        (e.From == layerEdge.To && e.To == layerEdge.From));

                    if (edge == null)
                    {
                        edges.Add(new Edge(layerEdge.From, layerEdge.To));
                    }
                    else
                    {
                        edge.Weight += includeWeights ? layerEdge.Weight : 1;
                    }
                }
            }

            foreach (var interLayerEdge in network.InterLayerEdges)
            {
                var edge = edges.FirstOrDefault(e =>
                    (e.From == interLayerEdge.From && e.To == interLayerEdge.To) ||
                    (e.From == interLayerEdge.To && e.To == interLayerEdge.From)
                );

                if (edge == null)
                {
                    edges.Add(new Edge(interLayerEdge.From, interLayerEdge.To));
                }
                else
                {
                    edge.Weight += includeWeights ? interLayerEdge.Weight : 1;
                }
            }

            return flattenedNetwork;
        }

        public Network Merge(Network network, int[] layerIndices, bool includeWeights)
        {
            var layers = network.Layers
                .Select((l, i) => (l, i))
                .Where(pair => layerIndices.Contains(pair.i))
                .Select(pair => pair.l)
                .ToList();
            var actors = network.Actors;
            var filteredNetwork = new Network(layers, actors);
            return Merge(filteredNetwork, includeWeights);
        }
    }
}
