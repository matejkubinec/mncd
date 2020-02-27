using System.Linq;
using Combinatorics.Collections;
using MNCD.Core;

namespace MNCD.Generators
{
    public class CompleteGraphGenerator
    {
        public Network Generate(int n)
        {
            var network = new Network();
            var actors = Enumerable
                .Range(0, n)
                .Select(i => new Actor(i.ToString()))
                .ToList();
            network.Actors = actors;
            network.Layers.Add(new Layer());

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
    }
}