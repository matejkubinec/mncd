using MNCD.Core;
using MNCD.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Flattening
{
    /// <summary>
    /// Class that implements local simplification flattening method.
    ///
    /// 6.1.2 Local Simplification - Page 103
    /// Multilayer Social Networks
    /// Mark E. Dickison, Matteo Magnani and Luca Rossi.
    /// </summary>
    public class LocalSimplification
    {
        /// <summary>
        /// Flattens multi-layer network based on local simplification method.
        /// </summary>
        /// <param name="network">Multi-layer network.</param>
        /// <param name="layerRelevances">Relevances of individual layers.</param>
        /// <param name="threshold">Treshold of relevance to be included.</param>
        /// <param name="weightEdges">Include edge weights.</param>
        /// <returns>Flattened network.</returns>
        public Network BasedOnLayerRelevance(Network network, double[] layerRelevances, double threshold, bool weightEdges = false)
        {
            var layerToIndex = network.GetLayerToIndex();
            var edgesDict = new Dictionary<(Actor from, Actor to), double>();

            if (network.Layers.Count != layerRelevances.Length)
            {
                throw new ArgumentException("Relevances count doesn't match the layers count.");
            }

            var flattenedNetwork = new Network(new Layer { Name = "Flattened" }, network.Actors);

            foreach (var layer in network.Layers)
            {
                var idx = layerToIndex[layer];
                var relevance = layerRelevances[idx];

                if (relevance >= threshold)
                {
                    foreach (var edge in layer.Edges)
                    {
                        var weight = edge.Weight * (weightEdges ? relevance : 1.0);

                        if (edgesDict.ContainsKey((edge.From, edge.To)))
                        {
                            edgesDict[(edge.From, edge.To)] += weight;
                        }
                        else if (edgesDict.ContainsKey((edge.To, edge.From)))
                        {
                            edgesDict[(edge.To, edge.From)] += weight;
                        }
                        else
                        {
                            edgesDict[(edge.From, edge.To)] = weight;
                        }
                    }
                }
            }

            foreach (var edge in network.InterLayerEdges)
            {
                var lf = layerToIndex[edge.LayerFrom];
                var lt = layerToIndex[edge.LayerTo];
                var relevance = (layerRelevances[lf] + layerRelevances[lt]) / 2.0;

                if (relevance >= threshold)
                {
                    var weight = edge.Weight * (weightEdges ? relevance : 1.0);

                    if (edgesDict.ContainsKey((edge.From, edge.To)))
                    {
                        edgesDict[(edge.From, edge.To)] += weight;
                    }
                    else if (edgesDict.ContainsKey((edge.To, edge.From)))
                    {
                        edgesDict[(edge.To, edge.From)] += weight;
                    }
                    else
                    {
                        edgesDict[(edge.From, edge.To)] = weight;
                    }
                }
            }

            flattenedNetwork.FirstLayer.Edges = edgesDict
                .Select(e => new Edge(e.Key.from, e.Key.to, e.Value))
                .ToList();

            return flattenedNetwork;
        }
    }
}
