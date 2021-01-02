using Combinatorics.Collections;
using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Generators
{
    /// <summary>
    /// Class for generating complete graphs.
    /// </summary>
    public class CompleteGraphGenerator
    {
        /// <summary>
        /// Generates complete graph with 'n' vertices.
        /// </summary>
        /// <param name="n">Number of vertices.</param>
        /// <returns>Complete graph of 'n' vertices.</returns>
        public Network Generate(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException("Argument 'n' must be greater than zero.");
            }

            var actors = Enumerable
                .Range(0, n)
                .Select(i => new Actor(i, i.ToString()))
                .ToList();
            var network = new Network(new Layer(), actors);

            if (n > 1)
            {
                var layer = network.Layers[0];
                var combinations = new Combinations<Actor>(actors, 2);
                foreach (var combination in combinations)
                {
                    var from = combination[0];
                    var to = combination[1];
                    var edge = new Edge(from, to);

                    layer.Edges.Add(edge);
                }
            }

            return network;
        }

        /// <summary>
        /// Generates multi-layer complete network of 'n' actors and 'l' layers.
        /// </summary>
        /// <param name="n">Number of actors.</param>
        /// <param name="l">Number of layers.</param>
        /// <returns>Complete multi-layer network.</returns>
        public Network GenerateMultiLayer(int n, int l)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException("Argument 'n' must be greater than zero.");
            }

            if (l <= 0)
            {
                throw new ArgumentOutOfRangeException("Argument 'l' must be greater than zero.");
            }

            var network = new Network(InitLayers(l), InitActors(n));
            var combinationsLayers = new Combinations<Layer>(network.Layers, 2);
            var combinationsActors = new Combinations<Actor>(network.Actors, 2);

            if (n > 1)
            {
                foreach (var combination in combinationsActors)
                {
                    var f = combination[0];
                    var t = combination[1];

                    foreach (var layer in network.Layers)
                    {
                        layer.Edges.Add(new Edge(f, t));
                    }

                    if (l > 1)
                    {
                        foreach (var lc in combinationsLayers)
                        {
                            var lf = lc[0];
                            var lt = lc[1];
                            network.InterLayerEdges.Add(new InterLayerEdge(f, lf, t, lt));
                        }
                    }
                }
            }

            if (l > 1)
            {
                foreach (var a in network.Actors)
                {
                    foreach (var lc in combinationsLayers)
                    {
                        var lf = lc[0];
                        var lt = lc[1];
                        network.InterLayerEdges.Add(new InterLayerEdge(a, lf, a, lt));
                    }
                }
            }

            return network;
        }

        private List<Actor> InitActors(int n) => Enumerable
            .Range(0, n)
            .Select(i => new Actor(i, i.ToString()))
            .ToList();

        private List<Layer> InitLayers(int l) => Enumerable
            .Range(0, l)
            .Select(l => new Layer($"l{l}"))
            .ToList();
    }
}