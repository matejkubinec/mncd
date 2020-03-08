using MNCD.Core;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Extensions
{
    public static class NetworkExtensions
    {
        public static Dictionary<Actor, Community> GetActorToCommunity(this Network network, List<Community> communities)
        {
            return network.Actors.ToDictionary(a => a, a => communities.First(c => c.Contains(a)));
        }

        public static Dictionary<Actor, double> GetActorToDegree(this Network network, bool weight = false)
        {
            var edges = network.Layers.First().Edges;
            return network.Actors.ToDictionary(a => a, a => edges.Where(e => e.From == a || e.To == a).Sum(e => weight ? e.Weight : 1.0));
        }

        public static Dictionary<Actor, List<Actor>> GetActorToNeighbours(this Network network)
        {
            var edges = network.Layers.First().Edges;
            return network.Actors.ToDictionary(a => a, a => edges.Where(e => e.From == a || e.To == a).Select(e => e.From != a ? e.From : e.To).ToList());
        }

        public static Dictionary<(Actor from, Actor to), double> GetEdgeToWeight(this Network network, int layerIdx)
        {
            var layer = network.Layers[layerIdx];
            return layer.Edges.SelectMany(e => new [] { e, e.Reverse() }).ToDictionary(e => (e.From, e.To), e => e.Weight);
        }

        public static double[,] LayerToAdjencyMatrix(this Network network, int layer)
        {
            var len = network.Actors.Count;
            var actorToIndex = network.Actors.Select((a, i) => (a, i)).ToDictionary(a => a.a, a => a.i);
            var matrix = new double[len, len];
            
            for(var i = 0; i < len; i++)
            {
                for(var j = 0; j < len; j++)
                {
                    matrix[i,j] = 0.0;
                }
            }

            foreach(var edge in network.Layers[layer].Edges)
            {
                var f = actorToIndex[edge.From];
                var t = actorToIndex[edge.To];
                var w = edge.Weight;

                matrix[f,t] = w;
                matrix[t,f] = w;
            }

            return matrix;
        }

        public static Dictionary<Actor, int> LayerDegreesDict(this Network network, int layer)
        {
            var deg = network.Actors.ToDictionary(a => a, a => 0);
            foreach(var edge in network.FirstLayer.Edges)
            {
                deg[edge.From]++;
                deg[edge.To]++;
            }
            return deg;
        }

        public static Dictionary<Layer, int> GetLayerToIndex(this Network network)
        {
            var i = 0;
            return network.Layers.ToDictionary(l => l, l => i++);
        }
    }
}
