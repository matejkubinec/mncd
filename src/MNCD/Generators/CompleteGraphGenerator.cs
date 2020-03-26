using Combinatorics.Collections;
using MNCD.Core;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Generators
{
    public class CompleteGraphGenerator
    {
        public Network Generate(int n)
        {
            var actors = Enumerable
                .Range(0, n)
                .Select(i => new Actor(i.ToString()))
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

        public Network GenerateMultiLayer(int n, int l)
        {
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

        internal List<Actor> InitActors(int n) => Enumerable
            .Range(0, n)
            .Select(i => new Actor(i.ToString()))
            .ToList();

        internal List<Layer> InitLayers(int l) => Enumerable
            .Range(0, l)
            .Select(l => new Layer($"l{l}"))
            .ToList();
    }
}