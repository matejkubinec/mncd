using MNCD.Core;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Flattening
{
    public class MergeFlattening
    {
        public Network Merge(Network network, bool includeWeights)
        {
            var edges = new List<Edge>();
            var flattenedNetwork = new Network()
            {
                Actors = network.Actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Flattened",
                        Edges = edges
                    },
                }
            };

            foreach (var layer in network.Layers)
            {
                foreach (var layerEdge in layer.Edges)
                {
                    var edge = edges.FirstOrDefault(e =>
                        (e.From == layerEdge.From && e.To == layerEdge.To) ||
                        (e.From == layerEdge.To && e.To == layerEdge.From)
                    );

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
            var filteredNetwork = new Network()
            {
                Actors = network.Actors,
                Layers = network.Layers
                    .Select((l, i) => (l, i))
                    .Where(pair => layerIndices.Contains(pair.i))
                    .Select(pair => pair.l)
                    .ToList()
            };

            return Merge(filteredNetwork, includeWeights);
        }
    }
}
