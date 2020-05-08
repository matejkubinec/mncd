using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MNCD.Core;

namespace MNCD.Neighbourhood
{
    /// <summary>
    /// Computes betweeness centrality for supplied network.
    ///
    /// A Faster Algorithm for Betweenness Centrality
    /// Ulrik Brandes
    /// https://kops.uni-konstanz.de/bitstream/handle/123456789/5739/algorithm.pdf.
    /// </summary>
    public static class Betweenness
    {
        public static Dictionary<Actor, int> Get(Network network)
        {
            var V = network.Actors;
            var N = network.FirstLayer.GetNeighboursDict();
            var cb = V.ToDictionary(v => v, v => 0);

            foreach (var s in V)
            {
                var S = new Stack<Actor>();
                var P = V.ToDictionary(v => v, v => new List<Actor>());
                var delta = V.ToDictionary(v => v, v => v == s ? 1 : 0);
                var d = V.ToDictionary(v => v, v => v == s ? 0 : -1);

                var Q = new Queue<Actor>();
                Q.Enqueue(s);

                while (Q.Count > 0)
                {
                    var v = Q.Dequeue();
                    S.Push(v);

                    foreach (var w in N[v])
                    {
                        // 'w' found for the first time?
                        if (d[w] < 0)
                        {
                            Q.Enqueue(w);
                            d[w] = d[v] + 1;
                        }

                        // shortest path to 'w' via 'v'?
                        if (d[w] == d[v] + 1)
                        {
                            delta[w] = delta[w] + delta[v];
                            P[w].Add(v);
                        }
                    }

                    var sigma = V.ToDictionary(v => v, v => 0);

                    // S returns vertices in order of non-increasing distance from s
                    while (S.Count > 0)
                    {
                        var w = S.Pop();

                        foreach (var p in P[w])
                        {
                            sigma[w] = sigma[p] + delta[p] / delta[w] * (1 + sigma[w]);
                        }

                        if (w != s)
                        {
                            cb[w] = cb[w] + sigma[w];
                        }
                    }
                }
            }

            return cb;
        }
    }
}