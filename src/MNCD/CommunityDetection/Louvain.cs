using MNCD.Attributes;
using MNCD.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MNCD.CommunityDetection
{
    // https://en.wikipedia.org/wiki/Louvain_modularity
    // https://perso.uclouvain.be/vincent.blondel/publications/08BG.pdf
    public class Louvain
    {
        public IEnumerable<IEnumerable<Actor>> Compute(Network network)
        {
            var actors = network.Actors;
            var layer = network.Layers[0];
            var actorEdges = actors.ToDictionary(a => a, a =>
            {
                if (layer.IsDirected)
                {
                    return layer
                        .Edges
                        .Where(e => e.From == a)
                        .Select(e => e.To)
                        .Distinct();
                }
                else
                {
                    return layer
                        .Edges
                        .Where(e => e.From == a || e.To == a)
                        .Select(e =>
                        {
                            if (e.From == a)
                            {
                                return e.To;
                            }
                            else
                            {
                                return e.From;
                            }
                        })
                        .Distinct();
                }
            });
            var communities = actors.ToDictionary(a => a, a => new List<Actor> { a });

            foreach (var a in actors)
            {
                foreach (var n in actorEdges[a])
                {
                    var neighbourCommunity = communities[n];

                    neighbourCommunity.Add(a);

                    communities.Remove(a);

                }
            }

            throw new NotImplementedException();


            return communities.Select(c => c.Value);
        }

        /// <summary>
        /// Computes modularity gain obtained by moving isolated node i into community c
        /// </summary>
        /// <param name="i">Isolated node</param>
        /// <param name="c">Community</param>
        /// <param name="neighbours">Actor to neighbours map</param>
        /// <param name="m">Sum of all weights in the network</param>
        /// <param name="isDirected">Denotes if the network is directed</param>
        /// <returns>Modularity gain obtained</returns>
        private double ModularityGain(Actor i, List<Actor> c, IDictionary<Actor, List<Actor>> neighbours, double m, bool isDirected)
        {
            // Sum of weights inside the community
            var sumIn = 0;
            var sumTot = 0;
            foreach (var actor in c)
            {
                sumIn += neighbours[actor].Where(n => c.Contains(n)).Count();
                sumTot += neighbours[actor].Where(n => !c.Contains(n)).Count();
            }

            var kIn = neighbours[i].Count();
            // var k
            throw new NotImplementedException();
        }
    }
}
