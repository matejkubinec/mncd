using System.Collections.Generic;
using System.Linq;
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

            if (totalPairs > 0)
            {
                return (intra + inter) / (double)totalPairs;
            }
            else
            {
                return 1.0;
            }
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
            var edgesDict = network.Layers.First().Edges.ToDictionary(e => (e.From, e.To), e => e);
            var inter = 0;
            for (var i = 0; i < communities.Count; i++)
            {
                foreach (var a1 in communities[i].Actors)
                {
                    for (var j = 0; j < communities.Count; j++)
                    {
                        if (i == j) continue;

                        foreach (var a2 in communities[j].Actors)
                        {
                            if (!edgesDict.ContainsKey((a1, a2)) && !edgesDict.ContainsKey((a2, a1)))
                            {
                                inter++;
                            }
                        }
                    }

                }

            }
            return inter;
        }
    }
}