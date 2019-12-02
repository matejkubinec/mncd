using MNCD.Core;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Flattening
{
    public class MergeFlattening
    {
        public Network Merge(Network network)
        {
            var edges = new List<Edge>();
            var isDirected = network.Layers.Any(l => l.IsDirected);
            var flattenedNetwork = new Network()
            {
                Actors = network.Actors,
                Layers = new List<Layer>
                {
                    new Layer
                    {
                        Name = "Flattened",
                        Edges = edges,
                        IsDirected = isDirected
                    },
                }
            };

            foreach (var layer in network.Layers)
            {
                foreach (var layerEdge in layer.Edges)
                {

                    if (isDirected)
                    {
                        if (layer.IsDirected)
                        {
                            var edge = edges.FirstOrDefault(e => e.From == layerEdge.From && e.To == layerEdge.To);

                            if (edge == null)
                            {
                                edges.Add(new Edge(layerEdge.From, layerEdge.To));
                            }
                            else
                            {
                                edge.Weight += 1;
                            }
                        }
                        else
                        {
                            var edge = edges.FirstOrDefault(e => e.From == layerEdge.From && e.To == layerEdge.To);
                            var edgeInverse = edges.FirstOrDefault(e => e.To == layerEdge.From && e.From == layerEdge.To);

                            if (edge == null)
                            {
                                edges.Add(new Edge(layerEdge.From, layerEdge.To));
                                edges.Add(new Edge(layerEdge.To, layerEdge.From));
                            }
                            else
                            {
                                edge.Weight += 1;
                                edgeInverse.Weight += 1;
                            }
                        }
                    }
                    else
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
                            edge.Weight += 1;
                        }
                    }
                }
            }

            return flattenedNetwork;
        }

        public Network Merge(Network network, int[] layerIndices)
        {
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

            foreach (var i in layerIndices)
            {
                flattenedNetwork.Layers[0].Edges = flattenedNetwork.Layers[0].Edges.Concat(network.Layers[i].Edges).ToList();
            }

            return flattenedNetwork;
        }
    }
}
