using MNCD.Core;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.Attributes
{
    // https://en.wikipedia.org/wiki/Modularity_(networks)
    public static class Modularity
    {

        public static double Compute(Layer layer, IDictionary<Actor, IEnumerable<Actor>> communities)
        {
            var actors = communities.Keys.ToList();
            var a = new Dictionary<(Actor, Actor), double>();
            var k = new Dictionary<Actor, double>();
            var m = 0;

            foreach (var edge in layer.Edges)
            {
                if (a.ContainsKey((edge.From, edge.To)))
                {
                    a[(edge.From, edge.To)] += 1;
                }
                else
                {
                    a[(edge.From, edge.To)] = 1;
                }

                if (k.ContainsKey(edge.From))
                {
                    k[edge.From] += 1;
                }
                else
                {
                    k[edge.From] = 1;
                }

                if (k.ContainsKey(edge.To))
                {
                    k[edge.To] += 1;
                }
                else
                {
                    k[edge.To] += 1;
                }

                if (a.ContainsKey((edge.To, edge.From)))
                {
                    a[(edge.To, edge.From)] += 1;
                }
                else
                {
                    a[(edge.To, edge.From)] = 1;
                }

                m += 1;
            }


            var sum = 0.0;

            for (var i = 0; i < communities.Keys.Count; i++)
            {
                for (var j = 0; j < communities.Keys.Count; j++)
                {
                    sum += (a[(actors[i], actors[j])] - (k[actors[i]] * k[actors[j]]) / (2 * m)) * KroneckerDelta(communities[actors[i]], communities[actors[j]]);
                }
            }

            return sum * (1 / (2 * m));
        }


        private static int KroneckerDelta(IEnumerable<Actor> ci, IEnumerable<Actor> cj)
        {
            return ci.All(cj.Contains) ? 1 : 0;
        }
    }
}

