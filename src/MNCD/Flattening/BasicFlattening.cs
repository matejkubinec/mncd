using MNCD.Core;
using System.Linq;

namespace MNCD.Flattening
{
    public class BasicFlattening
    {
        public Network Flatten(Network network, bool weightEdges = false)
        {
            var flattenedLayer = new Layer();

            foreach (var layer in network.Layers)
            {
                foreach (var edge in layer.Edges)
                {
                    var flattenedEdge = flattenedLayer.Edges
                        .FirstOrDefault(e => HasUndirectedEdge(e, edge));

                    if (flattenedEdge != null)
                    {
                        if (weightEdges)
                        {
                            flattenedEdge.Weight += 1;
                        }
                    }
                    else
                    {
                        flattenedLayer.Edges.Add(new Edge
                        {
                            From = edge.From,
                            To = edge.To,
                            Weight = 1,
                        });
                    }
                }
            }

            foreach (var interLayerEdge in network.InterLayerEdges)
            {
                flattenedLayer.Edges.Add(new Edge
                {
                    From = interLayerEdge.From,
                    To = interLayerEdge.To,
                    Weight = 1,
                });
            }

            var flattened = new Network
            {
                Actors = network.Actors,
            };
            flattened.Layers.Add(flattenedLayer);
            return flattened;
        }

        private bool HasUndirectedEdge(Edge layerEdge, Edge edge) =>
            (layerEdge.From == edge.From && layerEdge.To == edge.To) ||
            (layerEdge.To == edge.From && layerEdge.From == edge.To);
    }
}
