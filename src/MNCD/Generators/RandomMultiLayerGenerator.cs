using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Generators
{
    /// <summary>
    /// Class implementing a random network generator.
    ///
    /// Based on:
    /// - Multi-Layer
    /// https://github.com/SkBlaz/Py3plex/blob/master/py3plex/core/random_generators.py
    /// - Single-Layer
    /// https://networkx.github.io/documentation/networkx-1.10/_modules/networkx/generators/random_graphs.html#fast_gnp_random_graph.
    /// </summary>
    public class RandomMultiLayerGenerator
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Generates random network with 'n' actors, 'l' layers.
        /// </summary>
        /// <param name="n">Number of actors.</param>
        /// <param name="l">Number of layers.</param>
        /// <param name="p">Probability of an edge.</param>
        /// <returns>Generated network.</returns>
        public Network Generate(int n, int l, double p)
        {
            if (p > 1.0 || p < 0.0)
            {
                throw new ArgumentOutOfRangeException("Probabilty must between 0.0 and 1.0.");
            }

            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of nodes must be greater than zero.");
            }

            if (l <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of layers must be greater than zero.");
            }

            var singleLayer = GenerateSingleLayer(n, p);
            var actors = singleLayer.Actors;
            var layers = InitLayers(l);
            var actorToLayer = ActorToLayer(actors, layers);
            var multiLayer = new Network(layers, actors);
            foreach (var edge in singleLayer.FirstLayer.Edges)
            {
                var f = edge.From;
                var t = edge.To;
                var lf = actorToLayer[f];
                var lt = actorToLayer[t];

                if (lf == lt)
                {
                    lf.Edges.Add(edge);
                }
                else
                {
                    var interLayer = new InterLayerEdge
                    {
                        From = f,
                        To = t,
                        LayerFrom = lf,
                        LayerTo = lt,
                        Weight = edge.Weight,
                    };
                    multiLayer.InterLayerEdges.Add(interLayer);
                }
            }

            return multiLayer;
        }

        /// <summary>
        /// Generates a single layer network.
        /// </summary>
        /// <param name="n">Number of actors.</param>
        /// <param name="p">Probability of an edge.</param>
        /// <returns>Generated network.</returns>
        public Network GenerateSingleLayer(int n, double p)
        {
            if (p > 1.0 || p < 0.0)
            {
                throw new ArgumentOutOfRangeException("Probabilty must between 0.0 and 1.0.");
            }

            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of nodes must be greater than zero.");
            }

            var actors = InitActors(n);
            var network = new Network(new Layer(), actors);
            var v = 1;
            var w = -1;
            var lp = Math.Log(1.0 - p);
            while (v < n)
            {
                var lr = Math.Log(1.0 - Random.NextDouble());
                w = w + 1 + ((int)(lr / lp));

                while (w >= v && v < n)
                {
                    w -= v;
                    v += 1;
                }

                if (v < n)
                {
                    var from = actors[v];
                    var to = actors[w];
                    var edge = new Edge(from, to);
                    network.FirstLayer.Edges.Add(edge);
                }
            }

            return network;
        }

        private List<Actor> InitActors(int n) => Enumerable
            .Range(0, n)
            .Select(a => new Actor(a, $"a{a}"))
            .ToList();

        private List<Layer> InitLayers(int l) => Enumerable
            .Range(0, l)
            .Select(a => new Layer($"l{a}"))
            .ToList();

        private Dictionary<Actor, Layer> ActorToLayer(List<Actor> actors, List<Layer> layers) =>
            actors.ToDictionary(a => a, a => layers[Random.Next(layers.Count)]);
    }
}