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
            return layer.Edges.SelectMany(e => layer.IsDirected ? new [] { e } : new [] { e, e.Reverse() }).ToDictionary(e => (e.From, e.To), e => e.Weight);
        }
    }
}
