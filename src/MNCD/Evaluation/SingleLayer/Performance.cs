using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using MNCD.Core;

namespace MNCD.Evaluation.SingleLayer
{
    public static class Performance
    {
        public static double Get(Network network, List<Community> communities)
        {
            var intra = GetIntraEdges(network, communities);
            var inter = GetInterEdges(network, communities);
            var n = network.Actors.Count;
            var totalPairs = (n * (n - 1)) / 2.0;
            return (intra + inter) / (double)totalPairs;
        }

        private static int GetIntraEdges(Network network, List<Community> communities)
        {
            var count = 0;
            foreach (var edge in network.Layers.First().Edges)
            {
                if (communities.Any(c => c.Actors.Contains(edge.From) && c.Actors.Contains(edge.To)))
                {
                    count++;
                }
            }
            return count;
        }

        private static int GetInterEdges(Network network, List<Community> communities)
        {
            var inter = 0;
            foreach (var community in communities)
            {
                var actors = community.Actors;
                var indexes = actors.Select((a, i) => i).ToArray();
                var possibleCount = (int)new Combinations<int>(indexes, 2).Count;
                var count = network.Layers.First().Edges.Count(e => actors.Contains(e.From) && actors.Contains(e.To));

                inter += possibleCount - count;
            }
            return inter;
        }
    }
}