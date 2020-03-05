using System;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Generators
{
    // TODO: add tests
    // https://github.com/SkBlaz/Py3plex/blob/master/py3plex/core/random_generators.py
    // https://networkx.github.io/documentation/networkx-1.10/_modules/networkx/generators/random_graphs.html#fast_gnp_random_graph
    public class RandomMultiLayerGenerator
    {
        private static Random Random = new Random();

        public Network Generate(int n, int l, double p)
        {
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
                        Weight = edge.Weight
                    };
                    multiLayer.InterLayerEdges.Add(interLayer);
                }
            }
            return multiLayer;
        }

        internal Network GenerateSingleLayer(int n, double p)
        {
            var actors = InitActors(n);
            var network = new Network(new Layer(), actors);
            var v = 1;
            var w = -1;
            var lp = Math.Log(1.0 - p);
            while (v <= n)
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

        internal List<Actor> InitActors(int n)
        {
            return Enumerable
                .Range(0, n)
                .Select(a => new Actor($"a{a}"))
                .ToList();
        }

        internal List<Layer> InitLayers(int l)
        {
            return Enumerable
                            .Range(0, l)
                            .Select(a => new Layer($"l{a}"))
                            .ToList();
        }

        internal Dictionary<Actor, Layer> ActorToLayer(
            List<Actor> actors,
            List<Layer> layers)
        {
            return actors.ToDictionary(
                a => a,
                a =>
                {
                    var idx = Random.Next(layers.Count);
                    return layers[idx];
                }
            );
        }
    }
}