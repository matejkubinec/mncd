using MNCD.Core;
using System.Linq;

namespace MNCD.Flattening
{
    public class BasicFlattening
    {
        public Network Flatten(Network network, bool weightEdges = false)
        {
            var isDirected = network.Layers.Any(l => l.IsDirected);
            var flattenedLayer = new Layer()
            {
                IsDirected = isDirected
            };

            foreach (var layer in network.Layers)
            {
                foreach (var edge in layer.Edges)
                {
                    if (isDirected)
                    {
                        if (layer.IsDirected)
                        {
                            var flattenedEdge = flattenedLayer.Edges.FirstOrDefault(e => HasDirectedEdge(e, edge));

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
                                    Weight = 1
                                });
                            }
                        }
                        else
                        {
                            var flattenedEdgeFrom = flattenedLayer.Edges.FirstOrDefault(e => HasDirectedEdge(e, edge));

                            if (flattenedEdgeFrom != null)
                            {
                                if (weightEdges)
                                {
                                    flattenedEdgeFrom.Weight += 1;
                                }
                            }
                            else
                            {
                                flattenedLayer.Edges.Add(new Edge
                                {
                                    From = edge.From,
                                    To = edge.To,
                                    Weight = 1
                                });
                            }

                            var flattenedEdgeTo = flattenedLayer.Edges.FirstOrDefault(e => HasDirectedEdge(edge, e));

                            if (flattenedEdgeTo != null)
                            {
                                if (weightEdges)
                                {
                                    flattenedEdgeTo.Weight += 1;
                                }
                            }
                            else
                            {
                                flattenedLayer.Edges.Add(new Edge
                                {
                                    From = edge.To,
                                    To = edge.From,
                                    Weight = 1
                                });
                            }
                        }
                    }
                    else
                    {
                        var flattenedEdge = flattenedLayer.Edges.FirstOrDefault(e => HasUndirectedEdge(e, edge));

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
                                Weight = 1
                            });
                        }
                    }
                }
            }

            foreach (var interLayerEdge in network.InterLayerEdges)
            {
                flattenedLayer.Edges.Add(new Edge
                {
                    From = interLayerEdge.From,
                    To = interLayerEdge.To,
                    Weight = 1
                });
            }

            var flattened = new Network
            {
                Actors = network.Actors
            };
            flattened.Layers.Add(flattenedLayer);
            return flattened;
        }

        private bool HasDirectedEdge(Edge layerEdge, Edge edge)
        {
            return layerEdge.From == edge.From && layerEdge.To == edge.To;
        }

        private bool HasUndirectedEdge(Edge layerEdge, Edge edge)
        {
            return layerEdge.From == edge.From && layerEdge.To == edge.To || layerEdge.To == edge.From && layerEdge.From == edge.To;
        }
    }
}
