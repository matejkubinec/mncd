using MNCD.Core;
using MNCD.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Evaluation
{
    public static class Modularity
    {
        // http://networksciencebook.com/chapter/9#modularity    
        public static double Compute(Network network, List<Community> communities)
        {
            var edges = network.FirstLayer.Edges;
            var L = (double)edges.Count();
            var LC = CommunityToLinkCount(edges, communities);
            var KC = CommunityToDegrees(network, communities);
            var M = 0.0;
            foreach(var c in communities)
            {
                M += (LC[c] / L) - Math.Pow(KC[c] / (2.0 * L), 2.0);
            }
            return M;
        }

        internal static Dictionary<Community, int> CommunityToLinkCount(
            List<Edge> edges,
            List<Community> communities)
        {
            var res = communities.ToDictionary(c => c, c => 0);
            foreach(var edge in edges)
            {
                foreach(var c in communities)
                {
                    if (c.Actors.Contains(edge.From) &&
                        c.Actors.Contains(edge.To))
                    {
                        res[c]++;
                    }
                }
            }
            return res;
        }

        internal static Dictionary<Community, int> CommunityToDegrees(
            Network network,
            List<Community> communities)
        {
            var atd = network.GetActorToDegree();
            var res = communities.ToDictionary(c => c, c => 0);
            foreach(var c in communities)
            {
                foreach(var a in c.Actors)
                {
                    res[c] += (int)atd[a];
                }
            }
            return res;
        }
    }
}

